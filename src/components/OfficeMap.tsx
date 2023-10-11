import React from 'react';
import { GoogleMap, LoadScript, MarkerF } from '@react-google-maps/api';
import { officeInformationData } from '../assets/OfficeInformationData';

const mapStyles = {
  height: '400px',
  width: '1000px',
};

function OfficeMap() {
    return (
        <LoadScript googleMapsApiKey="KEY">
            <GoogleMap
                mapContainerStyle={mapStyles}
                center={officeInformationData['Utrecht'].officeInformation.coords}
                zoom={5}
            >
                {Object.values(officeInformationData).map((office) => (
                    <MarkerF
                        key={office.officeName}
                        position={ office.officeInformation.coords }
                        title={office.officeName} />
                ))}
            </GoogleMap>
        </LoadScript>
    );
}

export default OfficeMap;
