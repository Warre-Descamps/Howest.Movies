import http from 'k6/http';
import { check, sleep } from 'k6';
import { baseUrl } from '.options.js';

export function movieGetTest() {
    let res = http.get(`${baseUrl}/api/movie`);
    check(res, { 'status was 200': (r) => r.status === 200 });
    sleep(1);
}