import React, { useState } from "react";
import LineDiagramFetcher from "../Components/LineDiagramFetcher.jsx";
import "./LineSelector.css";
import { capitalize } from "../../utility/capitalize.jsx";

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

    const [selectedLine, setSelectedLine] = useState("bakerloo");
   

    return (
        <div className="row">
            {
                lines.map(line => (
                    <div
                        key={line}
                        className={`box ${line}`}
                        onClick={() => setSelectedLine(line)}
                        aria-selected={selectedLine === line}
                    >
                        {capitalize(line)}
                    </div>

                ))
            }

            <div className="diagram-container">
                <LineDiagramFetcher lineName={selectedLine} />
            </div> 
        </div>
    );
}



