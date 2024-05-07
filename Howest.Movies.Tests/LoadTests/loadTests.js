import { movieGetTest } from './movieGetTest.js';
import { movieGetTopTest } from './movieGetTopTest.js';
import { movieGetPosterTest } from './movieGetPosterTest.js';
import { movieGetReviewTest } from './movieGetReviewTest.js';

export let options = {
    vus: 10,
    duration: '30s'
};

export default function () {
    movieGetTest();
    movieGetTopTest();
    movieGetPosterTest();
    movieGetReviewTest();
}