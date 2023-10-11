import { GoogleMap, MarkerF, useLoadScript } from '@react-google-maps/api';
import { officeInformationData } from '../assets/OfficeInformationData';

const mapStyles = {
    height: '400px',
    width: '1000px',
};

interface OfficeMapProps {
    switchOffice: (officeName: string) => void;
}

function OfficeMap({ switchOffice }: OfficeMapProps) {
    const { isLoaded } = useLoadScript({
        googleMapsApiKey: 'AIzaSyDSE5J50qb82mBZfTGZheGJ0Jg8_vnk0_o',
    });

    if (isLoaded) {
        return (
            <GoogleMap
                mapContainerStyle={mapStyles}
                center={officeInformationData['Utrecht'].officeInformation.coords}
                zoom={7}>
                {Object.values(officeInformationData).map((office) => (
                    <MarkerF
                        key={office.officeName}
                        position={office.officeInformation.coords}
                        title={office.officeName}
                        onClick={() => switchOffice(office.officeName)} />
                ))}
            </GoogleMap>
        );
    }
    return (<h1>Google Maps are loading...</h1>);
}

export default OfficeMap;
