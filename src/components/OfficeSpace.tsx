import "../css/components/office-space.css";
import {
    BodySmall,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useContext, useEffect, useState } from "react";
import CurrentOfficeContext from "../contexts/CurrentOfficeContext";
import { GetDeskReservationForOfficeDate } from "../api/DeskReservationAPI";

interface IDeskCluster {
    clusterId: number;
    desks: Desk[];
}

export class Desk {
    clusterId: number
    deskId: number;
    occupiedMorning: boolean;
    occupiedAfternoon: boolean;
    isSelected: boolean;

    constructor(clusterId: number, deskId: number, occupiedMorning: boolean, occupiedAfternoon: boolean) {
        this.clusterId = clusterId;
        this.deskId = deskId;
        this.occupiedMorning = occupiedMorning;
        this.occupiedAfternoon = occupiedAfternoon;
        this.isSelected = false;
    }

    getState(): number {
        if (this.isSelected) return 1
        if (this.occupiedMorning && this.occupiedAfternoon) return 3
        if (!this.occupiedMorning && !this.occupiedAfternoon) return 0
        if (this.occupiedMorning || this.occupiedAfternoon) return 2
        return -1
    }
}

function OfficeSpace() {
    const officeName = useContext(CurrentOfficeContext).currentOffice;
    const [displayedDate, setDisplayedDate] = useState(new Date());
    const [selectedDesk, setSelectedDesk] = useState<Desk | undefined>(undefined);
    const [checkboxValues, setCheckboxValues] = useState([false, false]);

    const [initialDeskClusters, setInitialDeskClusters] = useState<IDeskCluster[]>([{ clusterId: 0, desks: [new Desk(0, 0, false, false), new Desk(0, 1, false, false), new Desk(0, 2, false, false), new Desk(0, 3, false, false),] },])
    // let originalDeskClusters: IDeskCluster[] =
    //     // [{ clusterId: 0, desks: [{ deskId: 0, state: 0 }, { deskId: 1, state: 4 }, { deskId: 2, state: 2 }, { deskId: 3, state: 3 },] },
    //     //  { clusterId: 1, desks: [{ deskId: 0, state: 0 }, { deskId: 1, state: 3 }, { deskId: 2, state: 4 }, { deskId: 3, state: 3 },] },
    //     //  { clusterId: 2, desks: [{ deskId: 0, state: 2 }, { deskId: 1, state: 0 }, { deskId: 2, state: 0 }, { deskId: 3, state: 3 },] },]
    //     // [{ clusterId: 0, desks: [new Desk(0, 0, true, true), new Desk(0, 1, false, true), new Desk(0, 2, true, false), new Desk(0, 3, false, false),] },]
    //     [{ clusterId: 0, desks: [new Desk(0, 0, false, false), new Desk(0, 1, false, false), new Desk(0, 2, false, false), new Desk(0, 3, false, false),] },]
    const [deskClusters, setDeskClusters] = useState<IDeskCluster[]>(initialDeskClusters)

    useEffect(() => {
        const GetDeskReservationForOfficeDateWraper = async () => {
            const reservations = await GetDeskReservationForOfficeDate(officeName, displayedDate);
            const emptyOriginalDeskClusters = [...initialDeskClusters]
            if (reservations.payload) {
                for (const desk of reservations.payload) {
                    if (desk.timeSlot == 0)
                        emptyOriginalDeskClusters[desk.clusterId].desks[desk.deskId].occupiedMorning = true;
                    if (desk.timeSlot == 1)
                        emptyOriginalDeskClusters[desk.clusterId].desks[desk.deskId].occupiedAfternoon = true;
                }
            }
            
            setDeskClusters(emptyOriginalDeskClusters);
            setInitialDeskClusters(emptyOriginalDeskClusters);
        }

        GetDeskReservationForOfficeDateWraper();
    }, [])

    const PreviousDay = () => {
        const newDate = new Date(displayedDate);
        const PreviousDayDate = new Date(newDate.setDate(newDate.getDate() - 1));
        if (
            new Date() < PreviousDayDate ||
            (new Date().getFullYear() == newDate.getFullYear() &&
                new Date().getMonth() == newDate.getMonth() &&
                new Date().getDate() == newDate.getDate())
        ) {
            setDisplayedDate(PreviousDayDate);
        }
    };
    const NextDay = () => {
        const newDate = new Date(displayedDate);
        const NextDayDate = new Date(newDate.setDate(newDate.getDate() + 1));

        // To calculate the time difference of two dates 
        const differenceInTime = new Date().getTime() - NextDayDate.getTime();

        // To calculate the no. of days between two dates 
        const differenceInDays = Math.abs(differenceInTime / (1000 * 3600 * 24));

        console.log(differenceInDays)

        if (differenceInDays <= 14) {
            setDisplayedDate(NextDayDate);
        }
    };

    const selectDesk = (desk: Desk) => {
        if(selectedDesk)
            selectedDesk.isSelected = false;

        if (selectedDesk?.clusterId == desk.clusterId && selectedDesk?.deskId == desk.deskId) {
            // Reset the state with the default values
            setDeskClusters(initialDeskClusters);
            setSelectedDesk(undefined)
            setCheckboxValues([false, false])
        }
        else if (desk.getState() == 0 || desk.getState() == 2) {
            const updatedDeskClusters = [...initialDeskClusters];

            // Toggle the class for the selected desk
            updatedDeskClusters[desk.clusterId].desks[desk.deskId].isSelected = true;

            // Update the state with the modified deskClusters, selected desk, checkboxes
            setDeskClusters(updatedDeskClusters);
            setSelectedDesk(desk)
            setCheckboxValues([desk.occupiedMorning, desk.occupiedAfternoon])
        }
    };

    const handleCheckboxChange = (index: number) => {
        const updatedCheckedBoxes: boolean[] = [...checkboxValues];
        updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
        setCheckboxValues(updatedCheckedBoxes);
    };

    const GetData = () => {
        console.log(officeName, displayedDate.toLocaleDateString(), selectedDesk, checkboxValues);
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
                {deskClusters.map((deskCluster) => (
                    <DeskCluster desks={deskCluster.desks} clusterId={deskCluster.clusterId} selectDesk={selectDesk} key={deskCluster.clusterId} />
                ))}
            </div>
            <div className="office-space__availability-bar">
                {selectedDesk &&
                    <>
                        <div className="availability-bar__times">
                            <BodySmall children="Morning" />
                            <BodySmall children="Afternoon" />
                        </div>
                        <div className="availability-bar__bars">
                            <div className={`availability-bar__bar availability-bar__bar${!selectedDesk?.occupiedMorning ? "--success" : "--fail"}`}></div>
                            <div className={`availability-bar__bar availability-bar__bar${!selectedDesk?.occupiedAfternoon ? "--success" : "--fail"}`}></div>
                        </div>
                        <form className="availability-bar__form body--normal">
                            <div className="availability-bar__checkboxes">
                                <input
                                    type="checkbox"
                                    checked={checkboxValues[0]}
                                    disabled={selectedDesk?.occupiedMorning}
                                    onChange={() => handleCheckboxChange(0)}
                                    id="morning"
                                />
                                <label htmlFor="morning">
                                    Morning
                                </label>
                            </div>
                            <div className="availability-bar__checkboxes">
                                <input
                                    type="checkbox"
                                    checked={checkboxValues[1]}
                                    disabled={selectedDesk?.occupiedAfternoon}
                                    onChange={() => handleCheckboxChange(1)}
                                    id="afternoon"
                                />
                                <label htmlFor="afternoon">
                                    Afternoon
                                </label>
                            </div>
                        </form>
                    </>}
            </div>
            <div className="office-space__info">
                <Button child="Book" onClick={() => (GetData())} />
            </div>
        </div>
    );
}

interface DeskClusterProps {
    clusterId: number;
    desks: Desk[];
    selectDesk: (desk: Desk) => void;
}

function DeskCluster({ clusterId, desks, selectDesk }: DeskClusterProps) {
    return (
        <div className="desk-cluster" id={clusterId.toString()}>
            {desks.map((desk) => (
                <DeskElement desk={desk} selectDesk={selectDesk} key={desk.deskId} />
            ))}
        </div>
    );
}

interface DeskProps {
    desk: Desk;
    selectDesk: (desk: Desk) => void;
}

function DeskElement({ desk, selectDesk }: DeskProps) {
    return (
        <div className="desk" id={desk.deskId.toString()} onClick={() => (selectDesk(desk))}>
            <div className="desk__desk">
                <div className={`desk__chair ${GetDeskState(desk.getState())}`}></div>
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
            return "desk__chair--selected";
            break;
        case 2:
            return "desk__chair--half-booked";
            break;
        case 3:
            return "desk__chair--fully-booked";
            break;
        case 4:
            return "desk__chair--unavailable";
            break;
        default:
            return "";
            break;
    }
}

export default OfficeSpace;
