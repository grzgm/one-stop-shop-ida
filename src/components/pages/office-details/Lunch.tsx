import { useState } from "react";
import { BodyNormal, BodySmall, HeadingLarge, HeadingSmall } from "../../text-wrapers/TextWrapers";
import Button from "../../Buttons";
import "../../../css/components/pages/office-details/lunch.css"

function Lunch() {
      const [weekRegistration, setWeekRegistration] = useState<boolean[]>([false, false, false, false, false]);
      const weekDaysNames = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']

      const handleCheckboxChange = (index: number) => {
            const updatedCheckedBoxes: boolean[] = [...weekRegistration];
            updatedCheckedBoxes[index] = !updatedCheckedBoxes[index];
            setWeekRegistration(updatedCheckedBoxes);
      };
      const saveLunchDays = () => {
            console.log(weekRegistration)
      };
      const registerForToday = () => {
            console.log("today register")
      };

      return (
            <div className="content">
                  <div className="description">
                        <HeadingLarge>Lunch</HeadingLarge>
                        <BodyNormal>Don't forget to register!</BodyNormal>
                  </div>
                  <main className="lunch-main">
                        <div className="lunch-main__recurring">
                              <HeadingSmall>Register recurring</HeadingSmall>
                              <BodySmall>Information will be sent</BodySmall>
                              <BodySmall>before 12:00 on the mentioned day</BodySmall>
                              <form className="lunch-main__form body--normal">
                                    {weekRegistration.map((isChecked, index) => (
                                          <div className="lunch-main__form__checkboxes">
                                                <input
                                                      type="checkbox"
                                                      checked={isChecked}
                                                      onChange={() => handleCheckboxChange(index)}
                                                      id={weekDaysNames[index]}
                                                />
                                                <label key={index} htmlFor={weekDaysNames[index]}>
                                                      {weekDaysNames[index]}
                                                </label>
                                          </div>
                                    ))}
                              </form>
                              <Button child="Save" onClick={() => saveLunchDays()} />
                        </div>
                        <div className="lunch-main__today">
                              <HeadingSmall>Register for today</HeadingSmall>
                              <BodySmall>Only for today</BodySmall>
                              <BodySmall>before 12:00</BodySmall>
                              <form>
                                    <Button child="Register" onClick={() => registerForToday()} />
                              </form>
                        </div>
                  </main>
            </div>
      );
}

export default Lunch;
