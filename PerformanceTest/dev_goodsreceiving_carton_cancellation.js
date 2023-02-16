import http from 'k6/http';
import { check } from 'k6';
import { SharedArray } from "k6/data";
import { scenario, vu } from 'k6/execution';
import { Trend, Counter } from 'k6/metrics';

const rcCreate = new Counter('request_count_sale');
const wtCreate = new Trend('waiting_time_sale');
const testEndpoint = 'https://pointofsaleapi.azurewebsites.net/Sales';
const ARTICLES = (new SharedArray("articles", function () { return JSON.parse(open('./articles.json')).articles; }))

//API call parameters
const PARAMS = {
    headers: {
        'accept': 'application/json',
        'Content-Type': 'application/json'
    },
    timeout: '120s'
};

//Checks the status code of the request
export function checkHttpStatusCode(response, expectedStatusCode) {
    check(response, {
        'expected status code': (r) => r.status === expectedStatusCode
    });

    if (response.status != expectedStatusCode && response.status != 0) {
        console.log("ErrorResponse: Status: " +
            JSON.stringify(response.status) + " - " + JSON.stringify(response.status_text) +
            ", URL: " + JSON.stringify(response.url) +
            ", Response Body: " + JSON.stringify(response.body).substring(0,1000) +
            ", Request Body: " + JSON.stringify(response.request.body).substring(0,1000));
    }
}

// Setup
export let options = {
    scenarios: {
        sendSale: {
            executor: 'per-vu-iterations',
            vus: 20,
            iterations: 30,
            maxDuration: '10m',
            tags: { test_type: 'sendSale' },
            exec: 'sendSale',
        }
    },
    discardResponseBodies: false,
    thresholds: { },
};


export function sendSale() {
    let request = {
        articleNumber : ARTICLES[Math.floor(Math.random() * ARTICLES.length) + 1],
        salesPrice : Math.floor(Math.random() * 200) + 10
    }

    var response = http.post(testEndpoint, JSON.stringify(request), PARAMS);

    if(response.timings != undefined){
        rcCreate.add(1);
        wtCreate.add(response.timings.waiting);
    }

    checkHttpStatusCode(response, 200);
};