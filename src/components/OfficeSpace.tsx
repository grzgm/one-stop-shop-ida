import "../css/components/office-space.css";
import {
    BodySmall,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useState } from "react";
import React from "react";

interface IDesk {
    state: number;
}

interface IDeskCluster {
    desks: IDesk[];
}

function OfficeSpace() {
    const [displayedDate, setDisplayedDate] = useState(new Date());
    const [selectedDesk, setSelectedDesk] = useState([-1, -1]);
    const [checkboxValues, setCheckboxValues] = useState([false, false]);

    const originalDeskClusters: IDeskCluster[] = [{ desks: [{ state: 0 }, { state: 4 }, { state: 2 }, { state: 3 },] },{ desks: [{ state: 0 }, { state: 3 }, { state: 4 }, { state: 3 },] },{ desks: [{ state: 2 }, { state: 0 }, { state: 0 }, { state: 3 },] },]
    const [deskClusters, setDeskClusters] = useState<IDeskCluster[]>(originalDeskClusters)

    const PreviousMonth = () => {
        const newDate = new Date(displayedDate);
        const previousMonthDate = new Date(
            newDate.setMonth(newDate.getMonth() - 1)
        );
        if (
            new Date() < previousMonthDate ||
            (new Date().getFullYear() == newDate.getFullYear() &&
                new Date().getMonth() == newDate.getMonth())
        ) {
            setDisplayedDate(previousMonthDate);
        }
    };
    const NextMonth = () => {
        const newDate = new Date(displayedDate);
        const NextMonthDate = new Date(newDate.setMonth(newDate.getMonth() + 1));
        setDisplayedDate(NextMonthDate);
    };
    const SelectDesk = (clusterId: number, deskId: number) => {
        if(deskClusters[clusterId].desks[deskId].state == 0 ||deskClusters[clusterId].desks[deskId].state ==  2)
        {
            const updatedDeskClusters = [...originalDeskClusters];
    
            // Toggle the class for the selected desk
            updatedDeskClusters[clusterId].desks[deskId].state = 1;
    
            // Update the state with the modified deskClusters
            setDeskClusters(updatedDeskClusters);
            setSelectedDesk([clusterId, deskId])
        }
    };

    const handleCheckboxChange = (index: number) => {
        const updatedCheckedBoxes: boolean[] = [...checkboxValues];
        updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
        setCheckboxValues(updatedCheckedBoxes);
    };

    const GetData = () => {
        console.log(displayedDate.toLocaleDateString().slice(3), checkboxValues, selectedDesk);
    }

    return (
        <div className="office-space body--normal">
            <div className="office-space__date-picker">
                <div className="office-space__date-picker__arrows" onClick={PreviousMonth}>
                    <KeyboardArrowLeftIcon fontSize="inherit" />
                </div>
                <div className="office-space__date-picker__date">
                    {displayedDate.toLocaleDateString().slice(3)}
                </div>
                <div className="office-space__date-picker__arrows" onClick={NextMonth}>
                    <KeyboardArrowRightIcon fontSize="inherit" />
                </div>
            </div>
            <div className="office-space__overview">
                {deskClusters.map((deskCluster, index) => (
                    <DeskCluster desks={deskCluster.desks} clusterId={index} selectDesk={SelectDesk} />
                ))}
            </div>
            <div className="office-space__availability-bar">
                <div className="availability-bar__times">
                    <BodySmall children="Morning" />
                    <BodySmall children="Afternoon" />
                </div>
                <div className="availability-bar__bar">
                    <div className="availability-bar__bar__morning"></div>
                    <div className="availability-bar__bar__afternoon"></div>
                </div>
                <form className="availability-bar__form body--normal">
                    <div className="availability-bar__checkboxes">
                        <input
                            type="checkbox"
                            checked={checkboxValues[0]}
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
                            onChange={() => handleCheckboxChange(1)}
                            id="afternoon"
                        />
                        <label htmlFor="afternoon">
                            Afternoon
                        </label>
                    </div>
                </form>
            </div>
            <div className="office-space__info">
                <Button child="Book" onClick={() => (GetData())} />
            </div>
        </div>
    );
}

interface DeskClusterProps {
    clusterId: number;
    desks: IDesk[];
    selectDesk: (clusterId: number, deskId: number) => void;
}

function DeskCluster({ clusterId, desks, selectDesk }: DeskClusterProps) {

    return (
        <div className="desk-cluster" id={clusterId.toString()}>
            {desks.map((desk, index) => (
                <Desk clusterId={clusterId} deskId={index} state={desk.state} selectDesk={selectDesk} />
            ))}
        </div>
    );
}

interface DeskProps {
    clusterId: number;
    deskId: number;
    state: number;
    selectDesk: (clusterId: number, deskId: number) => void;
}

function Desk({ clusterId, deskId, state, selectDesk }: DeskProps) {

    return (
        <div className="desk" id={deskId.toString()} onClick={() => (selectDesk(clusterId, deskId))}>
            <div className="desk__desk">
                <div className={`desk__chair ${GetDeskState(state)}`}></div>
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
