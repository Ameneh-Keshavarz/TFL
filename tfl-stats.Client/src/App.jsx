import { Routes, Route, Link, useLocation } from 'react-router-dom';
import LineStatusTable from './Line';
import JourneyPlanner from './Journey';
import CentralLineDemo from './CentralLineDemo';
import './App.css';

function App() {
    const location = useLocation();

    return (
        <div className="app-container">
            <header className="main-header">
                <h1 className="site-title">🚇 London Travel Assistant</h1>
                <nav className="nav-bar">
                    <Link
                        to="/journey"
                        className={`nav-link ${location.pathname === '/journey' ? 'active' : ''}`}
                    >
                        Journey Planner
                    </Link>
                    <Link
                        to="/lines"
                        className={`nav-link ${location.pathname === '/lines' ? 'active' : ''}`}
                    >
                        Line Status
                    </Link>
                    <Link
                        to="/CentralLineDemo"
                        className={`nav-link ${location.pathname === '/CentralLineDemo' ? 'active' : ''}`}
                    >
                            Central Line Demo
                    </Link>
                </nav>
            </header>

            <main className="main-content">
                <Routes>
                    <Route path="/" element={<JourneyPlanner />} />
                    <Route path="/journey" element={<JourneyPlanner />} />
                    <Route path="/lines" element={<LineStatusTable />} />
                    <Route path="/CentralLineDemo" element={<CentralLineDemo />} />
                </Routes>
            </main>
        </div>
    );
}

export default App;
