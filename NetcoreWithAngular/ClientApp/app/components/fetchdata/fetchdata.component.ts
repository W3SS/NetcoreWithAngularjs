import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    public forecasts: any;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        http.get('http://10.112.13.211:63390/' + 'api/values').subscribe(result => {
            this.forecasts = result.json() as string[];
        }, error => console.error(error));
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
