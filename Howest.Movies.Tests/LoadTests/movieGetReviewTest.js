import http from 'k6/http';
import { check, sleep } from 'k6';
import { baseUrl } from '.options.js';

export function movieGetReviewTest() {
    let id = 'replace-with-valid-id'; // replace with a valid id
    let res = http.get(`${baseUrl}/api/movie/${id}/review`);
    check(res, { 'status was 200': (r) => r.status === 200 });
    sleep(1);
}