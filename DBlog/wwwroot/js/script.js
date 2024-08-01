document.addEventListener("DOMContentLoaded", function () {
  var navItems = document.querySelectorAll(".nav_li");
  navItems.forEach(function (item) {
    item.classList.remove("active");
  });

  if (currentUrl.includes("/article")) {
    document.getElementById("articleIndexLi").classList.add("active");
  } else if (currentUrl === "/" || currentUrl.includes("/Article/Home")) {
    document.getElementById("homeIndexLi").classList.add("active");
  } else if (currentUrl.includes("/article/myblog")) {
    document.getElementById("myblogLi").classList.add("active");
  } else if (currentUrl.includes("/contact/contact")) {
    document.getElementById("contactIndexLi").classList.add("active");
  } else if (currentUrl.includes("/article/getcomments")) {
    document.getElementById("commentsIndexLi").classList.add("active");
  }
});
