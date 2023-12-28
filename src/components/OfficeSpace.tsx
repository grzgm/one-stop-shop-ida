import "../css/components/office-space.css";
import {
    BodySmall,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useContext, useEffect, useState } from "react";
import CurrentOfficeContext from "../contexts/CurrentOfficeContext";
import { DeleteDeskReservation, GetDeskReservationOfficeLayout, GetDeskReservationsForOfficeDate, IDesk, IDeskReservationItem, IDeskReservationsDay, PostDeskReservation } from "../api/DeskReservationAPI";
import AlertContext from "../contexts/AlertContext";

export class Desk {
    clusterId: string
    deskId: string;
    occupied: boolean[];
    userReservations: boolean[];

    constructor(clusterId: string, deskId: string, occupied: boolean[], userReservations: boolean[]) {
        this.clusterId = clusterId;
        this.deskId = deskId;
        this.occupied = occupied;
        this.userReservations = userReservations;
    }
}
class DeskCluster {
    clusterId: string;
    desks: { [key: string]: Desk };

    constructor(clusterId: string, iDesks: IDesk[]) {
        this.clusterId = clusterId;
        this.desks = {}
        for (const deskId in iDesks) {
            this.desks[deskId] = new Desk(iDesks[deskId].clusterId, iDesks[deskId].deskId, iDesks[deskId].occupied, iDesks[deskId].userReservations)
        }
    }
}

function OfficeSpace() {
    const officeName = useContext(CurrentOfficeContext).currentOffice;
    const { setAlert } = useContext(AlertContext);

    const [displayedDate, setDisplayedDate] = useState(new Date());
    const [selectedDesk, setSelectedDesk] = useState<{ clusterId: string, deskId: string } | undefined>(undefined);
    const [checkboxValues, setCheckboxValues] = useState([false, false]);

    const [deskLayout, setDeskLayout] = useState<{ [key: string]: DeskCluster }>({})
    const [allDeskReservations, setAllDeskReservations] = useState<{ [key: string]: IDeskReservationsDay }>({})
    const [currentOfficeDesks, setCurrentOfficeDesks] = useState<{ [key: string]: DeskCluster }>({})

    useEffect(() => {
        SetUpOfficeSpace();
    }, [])

    const SetUpOfficeSpace = async () => {
        const startDate = new Date();
        const endDate = new Date(startDate);
        endDate.setDate(startDate.getDate() + 14);
        const dateIndex = new Date().toISOString().split('T')[0] + 'T00:00:00';

        // get general reservations
        const newDeskLayoutRes = await GetDeskReservationOfficeLayout(officeName);

        const newDeskLayout: { [key: string]: DeskCluster } = {};

        if (newDeskLayoutRes.payload) {
            for (const clusterId in newDeskLayoutRes.payload) {
                newDeskLayout[clusterId] = new DeskCluster(clusterId, newDeskLayoutRes.payload[clusterId].desks)
            }
        }

        setDeskLayout(newDeskLayout)

        const newAllDeskReservationsRes = await GetDeskReservationsForOfficeDate(officeName, startDate, endDate);
        let todayOccupied: IDeskReservationItem[] = [];
        let todayUserReservations: IDeskReservationItem[] = [];

        if (newAllDeskReservationsRes.payload) {
            setAllDeskReservations(newAllDeskReservationsRes.payload)
            todayOccupied = newAllDeskReservationsRes.payload[dateIndex].occupied
            todayUserReservations = newAllDeskReservationsRes.payload[dateIndex].userReservations
        }

        const newCurrentOfficeDesks = structuredClone(newDeskLayout)

        for (const occupation of todayOccupied) {
            newCurrentOfficeDesks[occupation.clusterId].desks[occupation.deskId].occupied[occupation.timeSlot] = true;
        }

        for (const reservation of todayUserReservations) {
            newCurrentOfficeDesks[reservation.clusterId].desks[reservation.deskId].userReservations[reservation.timeSlot] = true;
        }

        setCurrentOfficeDesks(newCurrentOfficeDesks)
    }

    const UpdateOfficeSpace = async (date: Date, updatedAllDeskReservations?: { [key: string]: IDeskReservationsDay }) => {
        date = structuredClone(date)
        updatedAllDeskReservations = updatedAllDeskReservations ? updatedAllDeskReservations : allDeskReservations
        const dateIndex = date.toISOString().split('T')[0] + 'T00:00:00'

        // get general reservations
        const newCurrentOfficeDesks = structuredClone(deskLayout)

        const todayOccupied = updatedAllDeskReservations[dateIndex].occupied;
        const todayUserReservations = updatedAllDeskReservations[dateIndex].userReservations;

        for (const occupation of todayOccupied) {
            newCurrentOfficeDesks[occupation.clusterId].desks[occupation.deskId].occupied[occupation.timeSlot] = true;
        }

        for (const reservation of todayUserReservations) {
            newCurrentOfficeDesks[reservation.clusterId].desks[reservation.deskId].userReservations[reservation.timeSlot] = true;
        }

        setAllDeskReservations(updatedAllDeskReservations)
        setCurrentOfficeDesks(newCurrentOfficeDesks)
    }

    const PreviousDay = async () => {
        const newDate = new Date(displayedDate);
        const PreviousDayDate = new Date(newDate.setDate(newDate.getDate() - 1));
        if (
            new Date() < PreviousDayDate ||
            (new Date().getFullYear() == newDate.getFullYear() &&
                new Date().getMonth() == newDate.getMonth() &&
                new Date().getDate() == newDate.getDate())
        ) {
            setSelectedDesk(undefined)
            setDisplayedDate(PreviousDayDate);
            await UpdateOfficeSpace(PreviousDayDate)
        }
    };
    const NextDay = async () => {
        const newDate = new Date(displayedDate);
        const NextDayDate = new Date(newDate.setDate(newDate.getDate() + 1));

        // To calculate the time difference of two dates 
        const differenceInTime = new Date().getTime() - NextDayDate.getTime();

        // To calculate the no. of days between two dates 
        const differenceInDays = Math.abs(differenceInTime / (1000 * 3600 * 24));

        if (differenceInDays <= 14) {
            setSelectedDesk(undefined)
            setDisplayedDate(NextDayDate);
            await UpdateOfficeSpace(NextDayDate)
        }
    };

    const selectDesk = (desk: Desk) => {
        if (selectedDesk?.clusterId == desk.clusterId && selectedDesk?.deskId == desk.deskId) {
            // Reset the state with the default values
            setSelectedDesk(undefined)
            setCheckboxValues([false, false])
        }
        else if (GetDeskState(desk) != 4 && GetDeskState(desk) != 5) {
            window.scrollTo({
                top: document.body.scrollHeight,
                behavior: 'smooth'
            });

            const newCheckboxValues: boolean[] = [];

            for (let i = 0; i < desk.occupied.length; i++) {
                newCheckboxValues.push(desk.occupied[i] || desk.userReservations[i]);
            }

            // Update the state with the modified deskClusters, selected desk, checkboxes
            setSelectedDesk({ clusterId: desk.clusterId, deskId: desk.deskId })
            setCheckboxValues(newCheckboxValues)
        }
    };

    const IsSelected = (clusterId: string, deskId: string) => {
        if (clusterId == selectedDesk?.clusterId && deskId == selectedDesk.deskId)
            return true;
        return false;
    }

    const handleCheckboxChange = (index: number) => {
        const updatedCheckedBoxes: boolean[] = [...checkboxValues];
        updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
        setCheckboxValues(updatedCheckedBoxes);
    };

    const Book = async () => {
        const dateIndex = displayedDate.toISOString().split('T')[0] + 'T00:00:00';
        const updatedAllDeskReservations = structuredClone(allDeskReservations)
        const reservations: number[] = [];
        const cancellation: number[] = [];
        if (selectedDesk) {
            // selected desk reference for quick ui updating 
            const selectedDeskRef = currentOfficeDesks[selectedDesk.clusterId].desks[selectedDesk.deskId]
            // update based on all checkboxes
            for (let i = 0; i < checkboxValues.length; i++) {
                // change values only if the checkboxes has been interacted with
                if (selectedDeskRef && !selectedDeskRef.occupied[i] && selectedDeskRef?.userReservations[i] != checkboxValues[i]) {
                    if (checkboxValues[i]) {
                        reservations.push(i);
                        selectedDeskRef.userReservations[i] = true;
                        updatedAllDeskReservations[dateIndex].userReservations.push({
                            isUser: true,
                            date: displayedDate,
                            clusterId: selectedDesk?.clusterId,
                            deskId: selectedDesk?.deskId,
                            timeSlot: i
                        });
                    }
                    else {
                        cancellation.push(i);
                        selectedDeskRef.userReservations[i] = false;
                        updatedAllDeskReservations[dateIndex].userReservations = updatedAllDeskReservations[dateIndex].userReservations.filter(item =>
                            item.isUser !== true ||
                            item.clusterId !== selectedDesk?.clusterId ||
                            item.deskId !== selectedDesk?.deskId ||
                            item.timeSlot !== i
                        );
                    }
                }
            }
            // for quick refresh
            setSelectedDesk({ ...selectedDesk })
            const apiCalls = [];
            // console.log(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, reservations, cancellation)
            if (reservations.length > 0) apiCalls.push(PostDeskReservation(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, reservations));
            if (cancellation.length > 0) apiCalls.push(DeleteDeskReservation(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, cancellation))
            const [reservationsRes, cancellationRes] = await Promise.all(apiCalls);

            // check for correct response, if incorrect go back to old values
            if ((!reservationsRes || reservationsRes.success) && (!cancellationRes || cancellationRes.success)) {
                await UpdateOfficeSpace(displayedDate, updatedAllDeskReservations);
                setAlert("Successfully Reserved.", reservationsRes.success);
            }
            else {
                await UpdateOfficeSpace(displayedDate);
                setAlert("Cannot Reserve the Desk.", reservationsRes.success);
            }
        }
    }

    return (
        <div className="office-space body--normal">
            <div className="office-space__date-picker">
                <div className="office-space__date-picker__arrows" onClick={PreviousDay}>
                    <KeyboardArrowLeftIcon fontSize="inherit" />
                </div>
                <div className="office-space__date-picker__date">
                    {displayedDate.toLocaleDateString()}
                </div>
                <div className="office-space__date-picker__arrows" onClick={NextDay}>
                    <KeyboardArrowRightIcon fontSize="inherit" />
                </div>
            </div>
            <div className="office-space__overview">
                {Object.keys(currentOfficeDesks).map((key) => (
                    <DeskClusterComponent desks={currentOfficeDesks[key].desks} clusterId={currentOfficeDesks[key].clusterId} selectDesk={selectDesk} key={currentOfficeDesks[key].clusterId} isSelected={IsSelected} />
                ))}
            </div>
            {selectedDesk &&
                <>
                    <div className="office-space__availability-bar">
                        <div className="availability-bar__times">
                            <BodySmall children="Morning" />
                            <BodySmall children="Afternoon" />
                        </div>
                        <div className="availability-bar__bars">
                            {currentOfficeDesks[selectedDesk.clusterId].desks[selectedDesk.deskId].occupied.map((isOccupied, index) => (
                                <div className={`availability-bar__bar availability-bar__bar${!isOccupied ? "--success" : "--fail"}`} key={index}></div>
                            ))}
                        </div>
                        <form className="availability-bar__form body--normal">
                            {currentOfficeDesks[selectedDesk.clusterId].desks[selectedDesk.deskId].occupied.map((isOccupied, index) => {
                                const onChange = () => {
                                    handleCheckboxChange(index);
                                    // Add your checkbox change logic here
                                };

                                return (
                                    <div className="availability-bar__checkboxes" key={index} id={index.toString()}>
                                        <input
                                            type="checkbox"
                                            checked={checkboxValues[index]}
                                            disabled={isOccupied}
                                            onChange={onChange}
                                            id={`morning-${index}`}
                                        />
                                    </div>
                                );
                            })}
                        </form>
                    </div>
                    <div className="office-space__info">
                        <Button child="Book" onClick={Book} />
                    </div>
                </>}
        </div>
    );
}

interface DeskClusterComponentProps {
    clusterId: string;
    desks: { [key: string]: Desk };
    selectDesk: (desk: Desk) => void;
    isSelected: (clusterId: string, deskId: string) => boolean;
}

function DeskClusterComponent({ clusterId, desks, selectDesk, isSelected }: DeskClusterComponentProps) {
    return (
        <div className="desk-cluster" id={clusterId.toString()}>
            {Object.keys(desks).map((index) => (
                <DeskComponent desk={desks[index]} selectDesk={selectDesk} key={desks[index].deskId} isSelected={isSelected} />
            ))}
        </div>
    );
}

interface DeskComponentProps {
    desk: Desk;
    selectDesk: (desk: Desk) => void;
    isSelected: (clusterId: string, deskId: string) => boolean;
}

function DeskComponent({ desk, selectDesk, isSelected }: DeskComponentProps) {
    return (
        <div className="desk" id={desk.deskId.toString()} onClick={() => (selectDesk(desk))}>
            <div className="desk__desk">
                <div className={`desk__chair ${GetDeskStateClassName(GetDeskState(desk))} ${isSelected(desk.clusterId, desk.deskId) ? GetDeskStateClassName(6) : ""}`}></div>
            </div>
        </div>
    );
}

function GetDeskState(desk: Desk): number {
    let amountOfOccupied = 0;
    for (const timeSlot of desk.occupied) {
        if (timeSlot) amountOfOccupied++;
    }
    let amountOfReserved = 0;
    for (const timeSlot of desk.userReservations) {
        if (timeSlot) amountOfReserved++;
    }

    if (desk.occupied.length == 0) return 5
    if (amountOfOccupied == desk.occupied.length) return 4
    if (amountOfOccupied > 0 && amountOfReserved > 0) return 3
    if (amountOfOccupied > 0 && amountOfReserved == 0) return 2
    if (amountOfOccupied == 0 && amountOfReserved > 0) return 1
    if (amountOfOccupied == 0 && amountOfReserved == 0) return 0
    return -1
}

function GetDeskStateClassName(state: number) {
    switch (state) {
        case 0:
            return "desk__chair--available";
            break;
        case 1:
            return "desk__chair--user-booked";
            break;
        case 2:
            return "desk__chair--half-booked";
            break;
        case 3:
            return "desk__chair--mix-booked";
            break;
        case 4:
            return "desk__chair--fully-booked";
            break;
        case 5:
            return "desk__chair--unavailable";
            break;
        case 6:
            return "desk__chair--selected";
            break;
        default:
            return "";
            break;
    }
}

export default OfficeSpace;
