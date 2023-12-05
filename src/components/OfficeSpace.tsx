import "../css/components/office-space.css";
import {
    BodySmall,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useContext, useEffect, useState } from "react";
import CurrentOfficeContext from "../contexts/CurrentOfficeContext";
import { GetDeskReservationForOfficeDate, IDesk } from "../api/DeskReservationAPI";

export class Desk {
    clusterId: number
    deskId: number;
    occupied: boolean[];
    isSelected: boolean;

    constructor(clusterId: number, deskId: number, occupied: boolean[]) {
        this.clusterId = clusterId;
        this.deskId = deskId;
        this.occupied = occupied;
        this.isSelected = false;
    }

    getState(): number {
        let amountOfOccupied = 0;
        for (const timeSlot of this.occupied) {
            if (timeSlot) amountOfOccupied++;
        }

        if (this.isSelected) return 1
        if (amountOfOccupied == this.occupied.length) return 3
        if (amountOfOccupied == 0) return 0
        else return 2
        return -1
    }
}
class DeskCluster {
    clusterId: number;
    desks: Desk[];

    constructor(clusterId: number, iDesks: IDesk[]) {
        this.clusterId = clusterId;
        this.desks = []

        for (const iDesk of iDesks) {
            this.desks.push(new Desk(iDesk.clusterId, iDesk.deskId, iDesk.occupied))
        }
    }
}

function OfficeSpace() {
    const officeName = useContext(CurrentOfficeContext).currentOffice;
    const [displayedDate, setDisplayedDate] = useState(new Date());
    const [selectedDesk, setSelectedDesk] = useState<Desk | undefined>(undefined);
    const [checkboxValues, setCheckboxValues] = useState([false, false]);
    const [initialDeskClusters, setInitialDeskClusters] = useState<DeskCluster[]>([])
    const [deskClusters, setDeskClusters] = useState<DeskCluster[]>(initialDeskClusters)

    useEffect(() => {
        const GetDeskReservationForOfficeDateWraper = async () => {
            const reservations = await GetDeskReservationForOfficeDate(officeName, displayedDate);
            const newDeskClusters: DeskCluster[] = [];

            if (reservations.payload) {
                for (const iDeskCluster of reservations.payload) {
                    newDeskClusters.push(new DeskCluster(iDeskCluster.clusterId, iDeskCluster.desks))
                }
            }

            setDeskClusters(newDeskClusters);
            setInitialDeskClusters(newDeskClusters);
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
        if (selectedDesk)
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
            setCheckboxValues(desk.occupied)
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
                    <DeskClusterComponent desks={deskCluster.desks} clusterId={deskCluster.clusterId} selectDesk={selectDesk} key={deskCluster.clusterId} />
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
                            {selectedDesk.occupied.map((isOccupied, index) => (
                                <div className={`availability-bar__bar availability-bar__bar${!isOccupied ? "--success" : "--fail"}`} key={index}></div>
                            ))}
                        </div>
                        <form className="availability-bar__form body--normal">
                            {selectedDesk.occupied.map((isOccupied, index) => {
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
                    </>}
            </div>
            <div className="office-space__info">
                <Button child="Book" onClick={() => (GetData())} />
            </div>
        </div>
    );
}

interface DeskClusterComponentProps {
    clusterId: number;
    desks: Desk[];
    selectDesk: (desk: Desk) => void;
}

function DeskClusterComponent({ clusterId, desks, selectDesk }: DeskClusterComponentProps) {
    return (
        <div className="desk-cluster" id={clusterId.toString()}>
            {desks.map((desk) => (
                <DeskComponent desk={desk} selectDesk={selectDesk} key={desk.deskId} />
            ))}
        </div>
    );
}

interface DeskComponentProps {
    desk: Desk;
    selectDesk: (desk: Desk) => void;
}

function DeskComponent({ desk, selectDesk }: DeskComponentProps) {
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
