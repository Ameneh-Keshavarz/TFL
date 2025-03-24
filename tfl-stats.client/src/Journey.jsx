import { useState } from 'react';
import './App.css';
function GetJourney() {

    const [formData, setFormData] = useState({ from: '', to: '' });

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
        console.log(e.target.value);


    }

    const handleSubmit = async (e) => {
        e.preventDefault();
        const response = await fetch('https://localhost:7056/api/Journey',
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

        if (response.ok) {
            const responseData = await response.json();
            console.log('Journey:', responseData);
        } else {
            console.error('Error sending journey data');
        }

    }

    return (
        <div>
            <h2>Plan Your Journey</h2>
            <form className="form" onSubmit={handleSubmit}>
                <label>From:</label>
                <input type="text" name="from" onChange={handleChange} required></input>


                <label>To:</label>
                <input type="text" name="to" onChange={handleChange} required></input>

                <br />

                <button className="button" type="submit">Plan Journey</button>

            </form>
        </div>);
}

export default GetJourney;