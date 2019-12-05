import React, { Component } from 'react';
import './Dartboard.css';
import opic from './XOpurp.png';
import xpicpurp from './Xpurp.png';
import xpicpurpx from './Xpurpx.png';
import opicblue from './XOblue.png';
import xpicblue from './Xblue.png';
import xpicbluex from './Xbluex.png';
import opicgreen from './XOgreen.png';
import xpicgreen from './Xgreen.png';
import xpicgreenx from './Xgreenx.png';
import opiclightblue from './XOlightblue.png';
import xpiclibluex from './XOlibluex.png';
import xpicliblue from './XOliblue.png';
import opicpink from './XOpink.png';
import xpicpink from './Xpink.png';
import xpicpinkx from './Xpinkx.png';
import opicorange from './XOorange.png';
import xpicorange from './Xorange.png';
import xpicorangex from './Xorangex.png';
import opicyellow from './XOyellow.png';
import xpicyellow from './Xyellow.png';
import xpicyellowx from './Xyellowx.png';

class Dartboard extends Component {
    constructor(props) {
        super(props);

        this.handleClick = this.handleClick.bind(this);

        this.state = {
            currentImage20: -1,
            currentImage19: -1,
            currentImage18: -1,
            currentImage17: -1,
            currentImage16: -1,
            currentImage15: -1,
            currentImageBull: -1,
            img20: [
                xpicpurp,
                xpicpurpx,
                opic
            ],
            img19: [
                xpicblue,
                xpicbluex,
                opicblue
            ],
            img18: [
                xpicliblue,
                xpiclibluex,
                opiclightblue
            ],
            img17: [
                xpicgreen,
                xpicgreenx,
                opicgreen
            ],
            img16: [
                xpicyellow,
                xpicyellowx,
                opicyellow
            ],
            img15: [
                xpicorange,
                xpicorangex,
                opicorange
            ],
            imgBull: [
                xpicpink,
                xpicpinkx,
                opicpink
            ]
        };
    }

    handleClick(id) {
        if (id === 'button20')
        if (this.state.currentImage20 < 2) {
            this.setState({
                currentImage20: (this.state.currentImage20 + 1) 
            })
        } else {
            return null;
        }
        else if (id === 'button19')
        if (this.state.currentImage19 < 2) {
            this.setState({
                currentImage19: (this.state.currentImage19 + 1)
            })
        } else {
            return null;
        }
        else if (id === 'button18')
        if (this.state.currentImage18 < 2) {
            this.setState({
                currentImage18: (this.state.currentImage18 + 1)
            })
        } else {
            return null;
        }
        else if (id === 'button17')
        if (this.state.currentImage17 < 2) {
            this.setState({
                currentImage17: (this.state.currentImage17 + 1)
            })
        } else {
            return null;
        }    
        else if (id === 'button16')
        if (this.state.currentImage16 < 2) {
            this.setState({
                currentImage16: (this.state.currentImage16 + 1)
            })
        } else {
            return null;
        }    
        else if (id === 'button15')
        if (this.state.currentImage15 < 2) {
            this.setState({
                currentImage15: (this.state.currentImage15 + 1)
            })
        } else {
            return null;
        }    
        else if (id === 'buttonBull')
        if (this.state.currentImageBull < 2) {
            this.setState({
                currentImageBull: (this.state.currentImageBull + 1)
            })
        } else { 
            return null;
        }
    }

       
    render() {
        return(
            <div className="dart-board">
                <div className="dart-header">
                Header
                </div>
                <div className="darter-name1">
                Player 1
                </div>
                <div className="darter-name2">
                Player 2
                </div>
                <div className="twenty-c1">
                    301
                </div>
                <div className="twenty-s1">
                    <img src={this.state.img20[this.state.currentImage20]} alt="" />
                </div>
                <div className="twenty-c2">
                    <button id='button20' onClick={() => this.handleClick('button20')} >20</button>
                </div>
                <div className="twenty-s2">
                    <img src={opic} alt="opic" />
                </div>
                <div className="twenty-c3">
                    301
                </div>
                <div className="nineteen-s1">
                    <img src={this.state.img19[this.state.currentImage19]} alt="" />
                </div>
                <div className="nineteen-c2">
                    <button id="buttons19" onClick={() => this.handleClick('button19')}>19</button>
                </div>
                <div className="nineteen-s2">
                    <img src={opicblue} alt="opicblue" />
                </div>
                <div className="eighteen-s1">
                    <img src={this.state.img18[this.state.currentImage18]} alt="" />
                </div>
                <div className="eighteen-c2">
                    <button id="button18" onClick={() => this.handleClick('button18')}>18</button>
                </div>
                <div className="eighteen-s2">
                    <img src={opiclightblue} alt="opiclightblue" />
                </div>
                <div className="seventeen-s1">
                    <img src={this.state.img17[this.state.currentImage17]} alt="" />
                </div>
                <div className="seventeen-c2">
                    <button id="button17" onClick={() => this.handleClick('button17')}>17</button>
                </div>
                <div className="seventeen-s2">
                    <img src={opicgreen} alt="opicgreen" />
                </div>
                <div className="sixteen-s1">
                    <img src={this.state.img16[this.state.currentImage16]} alt="" />
                </div>
                <div className="sixteen-c2">
                    <button id="button16" onClick={() => this.handleClick('button16')}>16</button>
                </div>
                <div className="sixteen-s2">
                    <img src={opicyellow} alt="opicyellow" />
                </div>
                <div className="fifteen-s1">
                    <img src={this.state.img15[this.state.currentImage15]} alt="" />
                </div>
                <div className="fifteen-c2">
                    <button id="button15" onClick={() => this.handleClick('button15')}>15</button>
                </div>
                <div className="fifteen-s2">
                    <img src={opicorange} alt="opicorange" />
                </div>
                <div className="bull-s1">
                    <img src={this.state.imgBull[this.state.currentImageBull]} alt="" />
                </div>
                <div className="bull-c2">
                    <button id="buttonBull" onClick={() => this.handleClick('buttonBull')}>B</button>
                </div>
                <div className="bull-s2">
                    <img src={opicpink} alt="opicpink" />
                </div>
                <div className="enter">
                    <button>Enter</button>
                </div>
                <div className="backspace">
                    <button>Delete</button>
                </div>
                <div className="triple">
                    <button>3x</button>
                </div>
                <div className="double">
                    <button>2x</button>
                </div>
                <div className="miss">
                    <button>Miss</button>
                </div>
               
            </div>
        );
    }
}

export default Dartboard;