const signInBtnLink = document.querySelector(".signInBtnLink");
const signUpBtnLink = document.querySelector(".signUpBtnLink");

const container = document.querySelector(".container");

signInBtnLink.addEventListener("click", () => {
  container.classList.toggle("active");
});
signUpBtnLink.addEventListener("click", () => {
  container.classList.toggle("active");
});
