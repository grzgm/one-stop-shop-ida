import "../css/components/calendar.css";
import {
  BodyNormal,
  BodySmall,
  HeadingLarge,
} from "./text-wrapers/TextWrapers";
import KeyboardArrowRightIcon from "@mui/icons-material/KeyboardArrowRight";
import KeyboardArrowLeftIcon from "@mui/icons-material/KeyboardArrowLeft";
import Button from "./Buttons";
import { useState } from "react";
import React from "react";

function Calendar() {
  const [displayedDate, setDisplayedDate] = useState(new Date());
  const [startSelected, setStartSelected] = useState<Date | null>(null);
  const [endSelected, setEndSelected] = useState<Date | null>(null);
  const [selectSwitch, setSelectSwitch] = useState(false);

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

  const SelectDate = (dayNumber: number) => {
    const selectedDate = new Date(
      displayedDate.getFullYear(),
      displayedDate.getMonth(),
      dayNumber
    );
    if (!selectSwitch) {
      setStartSelected(selectedDate);
      setSelectSwitch(true);
    } else {
      setEndSelected(selectedDate);
      setSelectSwitch(false);
    }
  };

  const GetData = (startSelected: Date | null, endSelected: Date | null) => {
      console.log(startSelected, endSelected);
  }

  return (
    <div className="calendar body--normal">
      <div className="calendar__date-picker">
        <div className="calendar__date-picker__arrows" onClick={PreviousMonth}>
          <KeyboardArrowLeftIcon fontSize="inherit" />
        </div>
        <div className="calendar__date-picker__date">
          {displayedDate.toLocaleDateString().slice(3)}
        </div>
        <div className="calendar__date-picker__arrows" onClick={NextMonth}>
          <KeyboardArrowRightIcon fontSize="inherit" />
        </div>
      </div>
      <div className="calendar__space">
        <CalendarMonth
          year={displayedDate.getFullYear()}
          month={displayedDate.getMonth() + 1}
          selectedStart={startSelected}
          selectedEnd={endSelected}
          selectDate={SelectDate}
        />
      </div>
      <div className="calendar__time-date-pickers">
        <div className="calendar__time-date-picker calendar__time-date-picker__start">
          Start:{" "}
          {startSelected == null ? "-" : startSelected.toLocaleDateString()}
        </div>
        <div className="calendar__time-date-picker calendar__time-date-picker__end">
          End: {endSelected == null ? "-" : endSelected.toLocaleDateString()}
        </div>
      </div>
      <div className="calendar__info">
        <BodySmall>Outlook and Slack will be automatically updated</BodySmall>
        <Button child="Book" onClick={() => (GetData(startSelected, endSelected))}/>
      </div>
    </div>
  );
}

function DaysInMonth(year: number, month: number): number {
  return new Date(year, month, 0).getDate();
}

function GetDayNumberInWeek(date: Date): number {
  let dayNumber = date.getDay();
  if (dayNumber === 0) {
    dayNumber = 7;
  }
  return dayNumber - 1;
}

function GenerateCalendarDays(
  year: number,
  month: number,
  selectedStart: Date | null,
  selectedEnd: Date | null,
  selectDate: (dayNumber: number) => void
) {
  let days = Array.from({ length: 42 }, () => <div />);
  const today = new Date();
  const currentMonthLength = DaysInMonth(year, month);
  const currentMonthStart = GetDayNumberInWeek(new Date(year, month - 1, 1));
  const previousMonthLength = DaysInMonth(year, month - 1);

  // Previous Month
  for (let i = currentMonthStart; i >= 0; i--) {
    days[i - 1] = (
      <div className="calendar__month__day calendar__month__day--disabled">
        {previousMonthLength - currentMonthStart + i}
      </div>
    );
  }

  // Current Month
  for (let i = 0; i < currentMonthLength; i++) {
    days[currentMonthStart + i] = (
      <div
        className="calendar__month__day calendar__month__day--default"
        onClick={() => selectDate(i + 1)}
      >
        {i + 1}
      </div>
    );
  }

  // Next Month
  for (let i = currentMonthStart + currentMonthLength; i < 42; i++) {
    days[i] = (
      <div className="calendar__month__day calendar__month__day--disabled">
        {i - currentMonthStart - currentMonthLength + 1}
      </div>
    );
  }

  if (today.getFullYear() == year && today.getMonth() + 1 == month) {
    const indexToday = currentMonthStart + today.getDate();
    days[indexToday] = React.cloneElement(days[indexToday], {
      className: `${days[indexToday].props.className} calendar__month__day--today`,
    });
  }

  if (
    selectedStart &&
    selectedStart.getFullYear() == year &&
    selectedStart.getMonth() + 1 == month
  ) {
    const indexSelected = currentMonthStart + selectedStart.getDate() - 1;
    days[indexSelected] = React.cloneElement(days[indexSelected], {
      className: `${days[indexSelected].props.className} calendar__month__day--selected`,
    });
    console.log(indexSelected);
  }

  if (
    selectedEnd &&
    selectedEnd.getFullYear() == year &&
    selectedEnd.getMonth() + 1 == month
  ) {
    const indexSelected = currentMonthStart + selectedEnd.getDate() - 1;
    days[indexSelected] = React.cloneElement(days[indexSelected], {
      className: `${days[indexSelected].props.className} calendar__month__day--selected`,
    });
  }

  return days;
}

interface CalendarMonthProps {
  year: number;
  month: number;
  selectedStart: Date | null;
  selectedEnd: Date | null;
  selectDate: (dayNumber: number) => void;
}

function CalendarMonth({
  year,
  month,
  selectedStart,
  selectedEnd,
  selectDate,
}: CalendarMonthProps) {
  const days = GenerateCalendarDays(
    year,
    month,
    selectedStart,
    selectedEnd,
    selectDate
  );
  return <div className="calendar__month">{days}</div>;
}

export default Calendar;
