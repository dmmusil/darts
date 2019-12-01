import React from 'react';
import {Link} from 'react-router-dom';
import './NavBar.css';
import logo from './darticorn.png';

function NavBar() {
    return(
        <nav className="navbar">
            <Link to="/"><img src={logo} alt="Logo" /></Link>
        </nav>
    );
}

export default NavBar;