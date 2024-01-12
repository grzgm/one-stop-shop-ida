import { GoogleMap, InfoWindow, MarkerF, useLoadScript } from '@react-google-maps/api';
import { IOfficeFeatures } from '../../../api/OfficeFeaturesAPI';

const mapStyles = {
    height: '400px',
    width: '100%',
};

interface OfficeMapProps {
    closestOfficeName?: string | undefined;
    officeFeatures: { [key: string]: IOfficeFeatures };
    switchOffice: (officeName: string) => void;
}

function OfficeMap({ closestOfficeName, officeFeatures, switchOffice }: OfficeMapProps) {
    const { isLoaded } = useLoadScript({
        // create .env.local file with VITE_GOOGLE_MAPS_API_KEY = "your_google_maps_api_key"
        googleMapsApiKey: import.meta.env.VITE_GOOGLE_MAPS_API_KEY ? import.meta.env.VITE_GOOGLE_MAPS_API_KEY : "",
    });

    const markers = []

    for (let office of Object.values(officeFeatures)) {
        if (closestOfficeName && office.officeName == closestOfficeName) {
            markers.push(
                <MarkerF
                    key={office.officeName}
                    position={office.officeInformation.officeCoordinates}
                    title={office.officeName}
                    onClick={() => switchOffice(office.officeName)}>
                    <InfoWindow position={office.officeInformation.officeCoordinates}>
                        <div>Closest Office</div>
                    </InfoWindow>
                </MarkerF>)
        }
        else {
            markers.push(
                <MarkerF
                    key={office.officeName}
                    position={office.officeInformation.officeCoordinates}
                    title={office.officeName}
                    onClick={() => switchOffice(office.officeName)} />)
        }
    }

    if (isLoaded) {
        return (
            <GoogleMap
                mapContainerStyle={mapStyles}
                center={officeFeatures['utrecht'].officeInformation.officeCoordinates}
                zoom={7}>
                {markers}
            </GoogleMap>
        );
    }
    return (<h1>Google Maps are loading...</h1>);
}

export default OfficeMap;
