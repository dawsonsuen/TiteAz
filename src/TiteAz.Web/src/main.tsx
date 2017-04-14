declare var  __webpack_public_path__:string;
__webpack_public_path__ = (window as any).rootLocation || '/';

//IE FIX: Must be first import in entry file.
import "babel-polyfill";
import "custom-event-polyfill";

// Load in css for the project
import './css/reset.css';
import "./css/bootstrap/bootstrap.less";
import './css/style.less';

import * as React from 'react'
import { render } from 'react-dom'
import { Container } from 'cerebral-view-react'
import controller from './controller'
import App from './components/Core'

render((
    <Container controller={controller} strict={false}>
        <App />
    </Container>
), document.getElementById("app"))