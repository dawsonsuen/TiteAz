import { Aurelia } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([{
            route: [ '', 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: '../home/home',
            nav: true,
            title: 'Home'
        }, {
            route: 'counter',
            name: 'counter',
            settings: { icon: 'education' },
            moduleId: '../counter/counter',
            nav: true,
            title: 'Counter'
        },
        {
            route: 'user/debts',
            name: 'fetchdebts',
            settings: { icon: 'th-list' },
            moduleId: '../user/debt/fetchdebt',
            nav: true,
            title: 'My Debts'
        }]);

        this.router = router;
    }
}
