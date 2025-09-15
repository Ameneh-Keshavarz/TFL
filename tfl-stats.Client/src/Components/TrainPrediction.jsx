import React from "react";
import "./TrainPrediction.css";

export default function TrainPrediction({
    destinationName,
    timeToStation
}) {
    return (
        <div className="train_in">
            <div className="train-destination">
                <span className="destination-name">{destinationName}</span>
                <span className="train-time">
                    {timeToStation !== null ? `${ConvertTime(timeToStation)}` : "No prediction"}
                </span>
            </div>
        </div>
    );
}

function ConvertTime(arrivalTime) {

    return (arrivalTime >= 60 ? `${Math.floor(arrivalTime / 60)} min` : `${ arrivalTime } sec`);
}
