import "../css/components/office-space.css";
import {
    BodySmall,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useContext, useEffect, useState } from "react";
import CurrentOfficeContext from "../contexts/CurrentOfficeContext";
import { DeleteDeskReservation, GetDeskReservationForOfficeDate, IDesk, PostDeskReservation } from "../api/DeskReservationAPI";

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

    GetState(): number {
        let amountOfOccupied = 0;
        for (const timeSlot of this.occupied) {
            if (timeSlot) amountOfOccupied++;
        }
        let amountOfReserved = 0;
        for (const timeSlot of this.userReservations) {
            if (timeSlot) amountOfReserved++;
        }

        if (this.occupied.length == 0) return 5
        if (amountOfOccupied == this.occupied.length) return 4
        if (amountOfOccupied > 0 && amountOfReserved > 0) return 3
        if (amountOfOccupied > 0 && amountOfReserved == 0) return 2
        if (amountOfOccupied == 0 && amountOfReserved > 0) return 1
        if (amountOfOccupied == 0 && amountOfReserved == 0) return 0
        return -1
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
    const [displayedDate, setDisplayedDate] = useState(new Date());
    const [selectedDesk, setSelectedDesk] = useState<{ clusterId: string, deskId: string } | undefined>(undefined);
    const [checkboxValues, setCheckboxValues] = useState([false, false]);
    const [initialDeskClusters, setInitialDeskClusters] = useState<{ [key: string]: DeskCluster }>({})
    const [deskClusters, setDeskClusters] = useState<{ [key: string]: DeskCluster }>(initialDeskClusters)

    useEffect(() => {
        SetUpOfficeSpace();
    }, [])

    const SetUpOfficeSpace = async (date?: Date) => {
        date = date ? date : displayedDate

        // get general reservations
        const reservations = await GetDeskReservationForOfficeDate(officeName, date);

        const newDeskClusters: { [key: string]: DeskCluster } = {};

        if (reservations.payload) {
            for (const clusterId in reservations.payload) {
                newDeskClusters[clusterId] = new DeskCluster(clusterId, reservations.payload[clusterId].desks)
            }
        }

        setDeskClusters(newDeskClusters);
        setInitialDeskClusters(newDeskClusters);
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
            await SetUpOfficeSpace(PreviousDayDate)
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
            await SetUpOfficeSpace(NextDayDate)
        }
    };

    const selectDesk = (desk: Desk) => {
        if (selectedDesk?.clusterId == desk.clusterId && selectedDesk?.deskId == desk.deskId) {
            // Reset the state with the default values
            setDeskClusters(initialDeskClusters);
            setSelectedDesk(undefined)
            setCheckboxValues([false, false])
        }
        else if (desk.GetState() != 4 && desk.GetState() != 5) {
            const updatedDeskClusters = { ...initialDeskClusters };
            const newCheckboxValues: boolean[] = [];

            for (let i = 0; i < desk.occupied.length; i++) {
                newCheckboxValues.push(desk.occupied[i] || desk.userReservations[i]);
            }

            // Update the state with the modified deskClusters, selected desk, checkboxes
            setDeskClusters(updatedDeskClusters);
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

    const GetData = async () => {
        const reservations: number[] = [];
        const cancellation: number[] = [];
        for (let i = 0; i < checkboxValues.length; i++) {
            const selectedDeskRef = selectedDesk ? deskClusters[selectedDesk.clusterId].desks[selectedDesk.deskId] : undefined
            if (selectedDeskRef && !selectedDeskRef.occupied[i] && selectedDeskRef?.userReservations[i] != checkboxValues[i]) {
                if (checkboxValues[i]) reservations.push(i)
                else cancellation.push(i)
            }
        }
        if (selectedDesk) {
            // console.log(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, reservations, cancellation)
            if (reservations.length > 0) await PostDeskReservation(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, reservations)
            if (cancellation.length > 0) await DeleteDeskReservation(officeName, displayedDate, selectedDesk?.clusterId, selectedDesk?.deskId, cancellation)
            await SetUpOfficeSpace(displayedDate)
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
                {Object.keys(deskClusters).map((index) => (
                    <DeskClusterComponent desks={deskClusters[index].desks} clusterId={deskClusters[index].clusterId} selectDesk={selectDesk} key={deskClusters[index].clusterId} isSelected={IsSelected} />
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
                            {deskClusters[selectedDesk.clusterId].desks[selectedDesk.deskId].occupied.map((isOccupied, index) => (
                                <div className={`availability-bar__bar availability-bar__bar${!isOccupied ? "--success" : "--fail"}`} key={index}></div>
                            ))}
                        </div>
                        <form className="availability-bar__form body--normal">
                            {deskClusters[selectedDesk.clusterId].desks[selectedDesk.deskId].occupied.map((isOccupied, index) => {
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
                        <Button child="Book" onClick={GetData} />
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
                <div className={`desk__chair ${GetDeskState(desk.GetState())} ${isSelected(desk.clusterId, desk.deskId) ? GetDeskState(6) : ""}`}></div>
            </div>
        </div>
    );
}

function GetDeskState(state: number) {
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
