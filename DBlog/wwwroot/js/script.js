document.addEventListener("DOMContentLoaded", function () {
  var currentUrl = window.location.pathname;

  // Tüm nav_li elemanlarının active sınıfını kaldır
  var navItems = document.querySelectorAll(".nav_li");
  navItems.forEach(function (item) {
    item.classList.remove("active");
  });

  // Geçerli URL'ye göre uygun nav_li elemanına active sınıfı ekle
  if (currentUrl === "/Article/Index") {
    document.getElementById("articleIndexLi").classList.add("active");
  } else if (currentUrl === "/") {
    document.getElementById("homeIndexLi").classList.add("active");
  } else if (currentUrl === "/Article/MyBlog") {
    document.getElementById("myblogLi").classList.add("active");
  } else if (currentUrl === "/Article/Contact") {
    document.getElementById("contactIndexLi").classList.add("active");
  } else if (currentUrl === "/Article/GetComments") {
    document.getElementById("commentsIndexLi").classList.add("active");
  } else if (currentUrl === "/Article/Details") {
    document.getElementById("articleIndexLi").classList.add("active");
  } else if (currentUrl === "/article/edit") {
    document.getElementById("articleIndexLi").classList.add("active");
  }
});
