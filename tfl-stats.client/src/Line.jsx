import { useEffect, useState } from 'react';
import React from 'react';
import './App.css';

function GetLine() {
    const [lines, setLines] = useState([]);
    const [showDelayedRows, setShowDelayedRows] = useState({});

    useEffect(() => {
        populateLineData();
    }, []);

    const getLineStatuses = (line) => {
        return line.lineStatuses;
    };

    const contents = lines === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
        : <table className="table" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                {lines.map((line, index) => {
                    const statuses = getLineStatuses(line);
                    const isDelayed = statuses.some(status => status.statusSeverityDescription.includes("Delay"));
                    const statusClass = isDelayed ? 'status-delayed' : 'status-good';

                    return (
                        <React.Fragment key={`${line.name}-group-${index}`}>
                            <tr>
                                <td>{line.name}</td>
                                <td className={statusClass} onClick={() => handleDisplayStatus(index)}>
                                    {statuses.map((status, i) => (
                                        <div key={i}>{status.statusSeverityDescription}</div>
                                    ))}
                                </td>
                            </tr>

                            {showDelayedRows[index] && isDelayed && (
                                <tr>
                                    <td colSpan="2" style={{ color: 'red' }}>
                                        {statuses.filter(status => status.statusSeverityDescription.includes("Delay")).map((status, i) => (
                                            <p key={i}>{status.reason}</p>
                                        ))}
                                    </td>
                                </tr>
                            )}
                        </React.Fragment>
                    );
                })}
            </tbody>
        </table>;

    return (
        <div className="container">
            {contents}
        </div>
    );

    async function populateLineData() {
        const response = await fetch('https://localhost:7056/api/LineStatusByMode');
        if (response.ok) {
            const data = await response.json();
            setLines(data);
            setShowDelayedRows({});
        }
    }

    function handleDisplayStatus(index) {
        setShowDelayedRows(prev => ({
            ...prev,
            [index]: !prev[index],
        }));
    }
}

export default GetLine;
