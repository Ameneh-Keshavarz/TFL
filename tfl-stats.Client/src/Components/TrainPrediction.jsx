import React from "react";
import "./TrainPrediction.css";

export function TrainPrediction({
    lineName,
    platformName,
    destinationName,
    destinationNaptanId,
    timeToStation
}) {
    return (
        <div className="train-card">
            <div className="train-header">
                <span className="train-line">{lineName} Line</span>
                <span className="train-platform">{platformName}</span>
            </div>
            <div className="train-destination">
                Towards <span className="destination-name">{destinationName}</span>
                <span className="destination-id"> ({destinationNaptanId})</span>
            </div>
            <div className="train-time">
                {timeToStation !== null ? `${timeToStation} seconds` : "No prediction"}
            </div>
        </div>
    );
}
