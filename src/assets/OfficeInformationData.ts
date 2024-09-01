import { IOfficeFeatures } from "../api/OfficeFeaturesAPI";

// export const officeInformationUtrechtDefaultData: { [key: string]: IOfficeFeatures } = {
//   utrecht: {
//     officeName: "utrecht",
//     canReserveDesk: true,
//     canRegisterLunch: true,
//     canRegisterPresence: true,
//     officeInformation: {
//       address: "Orteliuslaan 25 3528BA",
//       officeCoordinates: {
//         lat: 52.0722482,
//         lng: 5.0746558,
//       },
//       openingHours: "6:00 - 21:30 working days",
//       accessInformation:
//         "You need a Creative Valley Papendorp access pass to enter the Creative Valley building and its Xplore Group office.",
//       parkingInformation:
//         "Xplore Group has a limited number of parking lots available in the parking  garage. Provide the license plate of your car to the Xplore Group HR team via office@email.nl if you want to make use of these parking lots.",
//       lunchInformation:
//         "Lunch is served in the Creative Valley Papendorp “brasserie” (canteen) at 12 AM. Report via email to office@email.nl that you will have lunch in the canteen to have you added to the iDA NL lunch list",
//     },
//   },
// }

export const officeInformationUtrechtDefaultData: { [key: string]: IOfficeFeatures } = {
  utrecht: {
    officeName: "utrecht",
    canReserveDesk: true,
    canRegisterLunch: true,
    canRegisterPresence: true,
    officeInformation: {
      address: "Orteliuslaan 25 3528BA",
      officeCoordinates: {
        lat: 52.0722482,
        lng: 5.0746558,
      },
      openingHours: "6:00 - 20:00 working days",
      accessInformation:
        "You need a Creative Valley Papendorp access pass to enter the Creative Valley building and its Xplore Group office.",
      parkingInformation:
        "Xplore Group has a limited number of parking lots available in the parking  garage. Provide the license plate of your car to the Xplore Group HR team via office@email.nl if you want to make use of these parking lots.",
      lunchInformation:
        "Lunch is served in the Creative Valley Papendorp “brasserie” (canteen) at 12 AM. Report via email to office@email.nl that you will have lunch in the canteen to have you added to the iDA NL lunch list",
    },
  },
  amsterdam: {
    officeName: "amsterdam",
    canReserveDesk: true,
    canRegisterLunch: true,
    canRegisterPresence: true,
    officeInformation: {
      address: "Cruquiusweg 110F 1019AK",
      officeCoordinates: {
        lat: 52.368519,
        lng: 4.9533974,
      },
      openingHours: "-",
      accessInformation:
        "You need a key to access the office building. To get a Amsterdam office key, send an e-mail to the Xplore Group HR team via office@email.nl. ",
      parkingInformation:
        "Xplore Group has a limited number of parking lots available in the Parking Cruquius Office parking  garage. Provide the license plate of your car to the Xplore Group HR team via office@email.nl if you want to make use of these parking lots and you will get a parking access pass.",
      lunchInformation:
        "Lunch is arranged by the Xplore office manager. Apply for lunch via Slack if you want to participate.",
    },
  },
  eindhoven: {
    officeName: "eindhoven",
    canReserveDesk: true,
    canRegisterLunch: false,
    canRegisterPresence: true,
    officeInformation: {
      address: "High Tech Campus 69 5656AE",
      officeCoordinates: {
        lat: 51.4085791,
        lng: 5.4538161,
      },
      openingHours: "6:30 - 20:00 working days",
      accessInformation:
        "You need a access pass to enter the building and its Xplore Group office. You can enter the office with a key. Please put the key back after usage... The access pass is arranged via Xplore Group HR and provided by the Badge Office. The Badge Office is located in building The Strip at the 1st floor and is opened Monday-Friday 9:00-13:00.",
      parkingInformation:
        "Parking is free in the parking garages of the HTC business park! You can park your car in Parking 6, that’s the closest to building.",
      lunchInformation:
        "You can lunch in one of the restaurants, or buy a packed lunch in supermarket AH-To-Go on the HTC Strip.",
    },
  },
  kontich: {
    officeName: "kontich",
    canReserveDesk: false,
    canRegisterLunch: false,
    canRegisterPresence: false,
    officeInformation: {
      address: "Prins boudewijnlaan 24e 2550",
      officeCoordinates: {
        lat: 51.1415231,
        lng: 4.4360194,
      },
      openingHours: "6:30 - 20:00 working days",
      accessInformation: "Main entrance.",
      parkingInformation: "No parking",
      lunchInformation: "-",
    },
  },
  hasselt: {
    officeName: "hasselt",
    canReserveDesk: false,
    canRegisterLunch: false,
    canRegisterPresence: false,
    officeInformation: {
      address: "Kempische Steenweg 311 3500",
      officeCoordinates: {
        lat: 50.9517131,
        lng: 5.3497578,
      },
      openingHours: "6:30 - 20:00 working days",
      accessInformation: "Main entrance.",
      parkingInformation: "No parking",
      lunchInformation: "-",
    },
  },
  merelbeke: {
    officeName: "merelbeke",
    canReserveDesk: false,
    canRegisterLunch: false,
    canRegisterPresence: false,
    officeInformation: {
      address: "Guldensporenpark 88 9820",
      officeCoordinates: {
        lat: 51.0053017,
        lng: 3.7520626,
      },
      openingHours: "6:30 - 20:00 working days",
      accessInformation: "Main entrance.",
      parkingInformation: "No parking",
      lunchInformation: "-",
    },
  },
};
