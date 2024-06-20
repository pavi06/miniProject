document.addEventListener("DOMContentLoaded", function() {
    window.addEventListener('scroll', revealOnScroll);
});

function revealOnScroll() {
    var sections = document.querySelectorAll('.sec');
    
    sections.forEach(function(section) {
      var sectop = section.getBoundingClientRect().top;
      var windowHeight = window.innerHeight;
      
      if (sectop < windowHeight/1.5) {
        section.classList.add('visible');
      } else {
        section.classList.remove('visible');
      }
    });
}

