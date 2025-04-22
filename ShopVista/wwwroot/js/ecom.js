document.addEventListener("DOMContentLoaded", () => {
    // DOM Elements
    const header = document.getElementById("header");
    const navToggle = document.getElementById("navToggle");
    const navMenu = document.getElementById("navMenu");
    const overlay = document.getElementById("overlay");
    const backToTop = document.getElementById("backToTop");
    const testimonialSlides = document.querySelectorAll(".testimonial-slide");
    const heroContent = document.querySelector(".hero-content");
    const categoryCards = document.querySelectorAll(".category-card");
    const productCards = document.querySelectorAll(".product-card");

    // Toggle Mobile Menu
    if (navToggle) {
        navToggle.addEventListener("click", () => {
            navMenu.classList.toggle("active");
            overlay.classList.toggle("active");
            document.body.style.overflow = navMenu.classList.contains("active") ? "hidden" : "";
        });
    }

    // Close Mobile Menu when clicking outside
    if (overlay) {
        overlay.addEventListener("click", () => {
            navMenu.classList.remove("active");
            overlay.classList.remove("active");
            document.body.style.overflow = "";
        });
    }

    // Scroll Event
    window.addEventListener("scroll", () => {
        // Sticky Header
        if (window.scrollY > 100) {
            header.classList.add("scrolled");
            backToTop.classList.add("show");
        } else {
            header.classList.remove("scrolled");
            backToTop.classList.remove("show");
        }
    });

    // Back to Top functionality
    if (backToTop) {
        backToTop.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: "smooth",
            });
        });
    }

    // Smooth scroll for navigation links
    document.querySelectorAll('a[href^="#"]').forEach((anchor) => {
        anchor.addEventListener("click", function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute("href"));
            if (target) {
                const headerHeight = header.offsetHeight;
                const elementPosition = target.getBoundingClientRect().top;
                const offsetPosition = elementPosition + window.pageYOffset - headerHeight;

                window.scrollTo({
                    top: offsetPosition,
                    behavior: "smooth",
                });

                // Close mobile menu if open
                navMenu.classList.remove("active");
                overlay.classList.remove("active");
                document.body.style.overflow = "";
            }
        });
    });

    // Countdown Timer
    function updateCountdown() {
        const now = new Date();
        const endDate = new Date(now.getFullYear(), now.getMonth(), now.getDate() + 3);
        const diff = endDate - now;

        const days = Math.floor(diff / (1000 * 60 * 60 * 24));
        const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((diff % (1000 * 60)) / 1000);

        document.getElementById("days").textContent = days.toString().padStart(2, "0");
        document.getElementById("hours").textContent = hours.toString().padStart(2, "0");
        document.getElementById("minutes").textContent = minutes.toString().padStart(2, "0");
        document.getElementById("seconds").textContent = seconds.toString().padStart(2, "0");
    }

    setInterval(updateCountdown, 1000);
    updateCountdown();

    // Animate testimonials
    let currentSlide = 0;
    function moveTestimonials() {
        testimonialSlides.forEach((slide, index) => {
            slide.style.transform = `translateX(${100 * (index - currentSlide)}%)`;
        });
    }

    moveTestimonials();

    setInterval(() => {
        currentSlide = (currentSlide + 1) % testimonialSlides.length;
        moveTestimonials();
    }, 5000);

    // Intersection Observer for animations
    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("fade-in");
                    observer.unobserve(entry.target);
                }
            });
        },
        { threshold: 0.1 }
    );

    document.querySelectorAll(".category-card, .product-card").forEach((el) => {
        observer.observe(el);
    });

    // Initialize GSAP animations
    if (typeof gsap !== "undefined") {
        gsap.registerPlugin(ScrollTrigger);

        // Hero animation
        gsap.to(heroContent, {
            opacity: 1,
            y: 0,
            duration: 1,
            ease: "power3.out",
        });

        // Animate category cards

    }
});


