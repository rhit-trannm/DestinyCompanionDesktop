/**
 * @fileoverview
 * Provides the JavaScript interactions for all pages.
 *
 * @author
 * PUT_YOUR_NAME_HERE
 */
var rhit = rhit || {};

rhit.main = function () {
  const queryString = window.location.search;
  const urlParams = new URLSearchParams(queryString);
  const code = urlParams.get('code');
  console.log(code);
  document.getElementById("code").innerHTML = `Your Code: ${code}`
  console.log("Ready");
};

rhit.main();
