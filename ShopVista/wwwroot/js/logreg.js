document.addEventListener('DOMContentLoaded', function () {
    // Password visibility toggle functionality
    const togglePasswordButtons = document.querySelectorAll('.password-toggle');

    togglePasswordButtons.forEach(button => {
        button.addEventListener('click', function () {
            const passwordField = this.previousElementSibling;
            const type = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordField.setAttribute('type', type);

            // Toggle icon
            if (type === 'password') {
                this.innerHTML = '<i class="fas fa-eye"></i>';
            } else {
                this.innerHTML = '<i class="fas fa-eye-slash"></i>';
            }
        });
    });

    // Form validation visual feedback
    const formInputs = document.querySelectorAll('.form-control');

    formInputs.forEach(input => {
        input.addEventListener('blur', function () {
            // Email validation
            if (this.type === 'email' && this.value) {
                const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (emailRegex.test(this.value)) {
                    this.classList.add('is-valid');
                    this.classList.remove('is-invalid');
                } else {
                    this.classList.add('is-invalid');
                    this.classList.remove('is-valid');
                }
            }

            // Password strength
            if (this.type === 'password' && this.id === 'Password' && this.value) {
                const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
                if (passwordRegex.test(this.value)) {
                    this.classList.add('is-valid');
                    this.classList.remove('is-invalid');
                } else {
                    this.classList.add('is-invalid');
                    this.classList.remove('is-valid');
                }
            }

            // Required fields
            if (this.required && !this.value) {
                this.classList.add('is-invalid');
            } else if (this.required && this.value) {
                this.classList.add('is-valid');
            }
        });
    });

    // Animated form submission
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const submitButton = this.querySelector('button[type="submit"]');
            submitButton.classList.add('loading');
            submitButton.innerHTML = 'Processing...';

            // We're not actually preventing the form submission here
            // as this is just for visual effect before the actual submission
            // e.preventDefault();

            // Simulate success for demo (remove this in actual implementation)
            // setTimeout(() => {
            //     submitButton.classList.remove('loading');
            //     submitButton.innerHTML = 'Success!';
            //     submitButton.style.backgroundColor = 'var(--success-color)';
            //     // Then actually submit the form after visual feedback
            //     this.submit();
            // }, 1500);
        });
    });

    // Entry animations
    const authCard = document.querySelector('.auth-card');
    if (authCard) {
        authCard.style.opacity = '0';
        authCard.style.transform = 'translateY(30px)';

        setTimeout(() => {
            authCard.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
            authCard.style.opacity = '1';
            authCard.style.transform = 'translateY(0)';
        }, 100);
    }

    // Apply sequential fade-in to form controls
    const formControls = document.querySelectorAll('.form-group');
    formControls.forEach((control, index) => {
        control.style.opacity = '0';
        control.style.transform = 'translateY(20px)';

        setTimeout(() => {
            control.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
            control.style.opacity = '1';
            control.style.transform = 'translateY(0)';
        }, 200 + (index * 100));
    });
});