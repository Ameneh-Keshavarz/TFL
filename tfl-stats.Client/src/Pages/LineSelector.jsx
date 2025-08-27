import React, { useState } from "react";
import LineFromJson from "./CentralLineDiagramFromJson";

export default function LineSelector() {
    const lines = [
        "bakerloo",
        "central",
        "circle",
        "district",
        "hammersmith-city",
        "jubilee",
        "metropolitan",
        "northern",
        "piccadilly",
        "victoria",
        "waterloo-city",
        "dlr",
        "elizabeth"
    ];

    const [selectedLine, setselectedLine] = useState("");

    return (
        <div className="row">
            {
                lines.map(line => (
                <div className={`box ${line}`} key={line} onClick={() => setselectedLine(line)}>
                    {capitalize(line)}
                </div>
                ))
            }

            <div className="diagram-container">
                <LineFromJson lineName={selectedLine} />
            </div>

            
        </div>
    );
}

function capitalize(str) {
    return str
        .split("-")
        .map(word => word.charAt(0).toUpperCase() + word.slice(1))
        .join("-");
}



