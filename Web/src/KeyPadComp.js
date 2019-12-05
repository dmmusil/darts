import React, { Component } from 'react';

class KeyPadComp extends Component {

    render() {
        return(
            <div className="calc-r1-c1"><button name="1" onClick={event => this.props.onClick(event.target.name)}>1</button></div>
            <div className="calc-r1-c2"><button name="2" onClick={event => this.props.onClick(event.target.name)}>2</button></div>
            <div className="calc-r1-c3"><button name="3" onClick={event => this.props.onClick(event.target.name)}>3</button></div>
            <div className="calc-r2-c1"><button name="4" onClick={event => this.props.onClick(event.target.name)}>4</button></div>
            <div className="calc-r2-c2"> <button name="5" onClick={event => this.props.onClick(event.target.name)}>5</button></div>
            <div className="calc-r2-c3"><button name="6" onClick={event => this.props.onClick(event.target.name)}>6</button></div>
            <div className="calc-r3-c1"><button name="7" onClick={event => this.props.onClick(event.target.name)}>7</button></div>
            <div className="calc-r3-c2"><button name="8" onClick={event => this.props.onClick(event.target.name)}>8</button></div>
            <div className="calc-r3-c3"><button name="9" onClick={event => this.props.onClick(event.target.name)}>9</button></div>
            <div className="calc-r4-c1"><button name="+" onClick={event => this.props.onClick(event.target.name)}>+</button></div>
            <div className="calc-r4-c2"><button name="0" onClick={event => this.props.onClick(event.target.name)}>0</button></div>
            <div className="calc-r4-c3"><button name="=" onClick={event => this.props.onClick(event.target.name)}>Enter</button></div>  
        );
    }
}

export default KeyPadComp;