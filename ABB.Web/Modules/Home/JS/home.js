
let carouselSlide;
let prevBtn;
let nextBtn;
let indicatorsContainer;
let currentIndex;
let intervalId;
let images;
let previewTimeout;
let previewContainer;

$(document).ready(function(){
    // DOM Elements
    carouselSlide = document.getElementById('carouselSlide');
    prevBtn = document.getElementById('prevBtn');
    nextBtn = document.getElementById('nextBtn');
    indicatorsContainer = document.getElementById('indicators');

    currentIndex = 0;
    intervalId = null;
    previewTimeout = null;
    previewContainer = null;

    ajaxGet("/Home/GetSlideShows", function(data){
        images = data;

        initCarousel();
    })
})

// Create preview container
function createPreviewContainer() {
    if (!previewContainer) {
        previewContainer = document.createElement('div');
        previewContainer.className = 'preview-container';
        previewContainer.style.cssText = `
                position: absolute;
                bottom: 50px;
                z-index: 100;
                background: rgba(0, 0, 0, 0.8);
                border-radius: 8px;
                padding: 10px;
                display: none;
                transition: opacity 0.3s ease;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
                max-width: 200px;
                transform: translateX(-50%);
            `;
        document.querySelector('.carousel-container').appendChild(previewContainer);
    }
}

// Function to show preview
function showPreview(index, indicatorElement) {
    clearTimeout(previewTimeout);

    previewTimeout = setTimeout(function() {
        if (!previewContainer) createPreviewContainer();

        const rect = indicatorElement.getBoundingClientRect();
        const containerRect = document.querySelector('.carousel-container').getBoundingClientRect();

        // Get image URL for preview
        const imageUrls = Array.from(carouselSlide.querySelectorAll('.carousel-image')).map(img => img.src);
        const previewUrl = imageUrls[index] || images[index];

        if (previewUrl) {
            previewContainer.innerHTML = `
                    <div style="width: 180px; height: 100px; overflow: hidden; border-radius: 4px;">
                        <img src="${previewUrl}" 
                             alt="Preview" 
                             style="width: 100%; height: 100%; object-fit: cover; border-radius: 4px;">
                    </div>
                `;

            // Position the preview
            const leftPosition = rect.left + (rect.width / 2) - containerRect.left;
            previewContainer.style.left = leftPosition + 'px';

            // Show preview with fade in effect
            previewContainer.style.display = 'block';
            previewContainer.style.opacity = '0';

            setTimeout(function() {
                previewContainer.style.opacity = '1';
            }, 10);
        }
    }, 300); // 300ms delay before showing preview
}

// Function to hide preview
function hidePreview() {
    clearTimeout(previewTimeout);

    if (previewContainer) {
        previewContainer.style.opacity = '0';
        setTimeout(function() {
            if (previewContainer) {
                previewContainer.style.display = 'none';
            }
        }, 300);
    }
}

// Initialize carousel
function initCarousel() {
    
    // Load images into carousel
    images.forEach((image, index) => {
        const imgElement = document.createElement('img');
        imgElement.className = 'carousel-image';
        imgElement.src = image;
        carouselSlide.appendChild(imgElement);
    });

    updateCarousel();
    setupEventListeners();
    startAutoSlide();
}

// Set up event listeners
function setupEventListeners() {
    prevBtn.addEventListener('click', showPrevSlide);
    nextBtn.addEventListener('click', showNextSlide);

    // Keyboard navigation
    document.addEventListener('keydown', (e) => {
        if (e.key === 'ArrowLeft') showPrevSlide();
        if (e.key === 'ArrowRight') showNextSlide();
    });

    // Pause auto-slide on hover
    carouselSlide.addEventListener('mouseenter', () => {
        if (intervalId) clearInterval(intervalId);
    });

    carouselSlide.addEventListener('mouseleave', () => {
        startAutoSlide();
    });

    indicatorsContainer.addEventListener('mouseleave', function() {
        hidePreview();
    });
}

// Update carousel display
function updateCarousel() {
    // Update slide position
    carouselSlide.style.transform = `translateX(-${currentIndex * 100}%)`;

    // Update indicators
    updateIndicators();

    // Update image info
    // const currentImage = images[currentIndex];
    // imageTitle.textContent = currentImage.title;
    // imageDescription.textContent = currentImage.description;
}

// Update indicators
function updateIndicators() {
    indicatorsContainer.innerHTML = '';

    images.forEach((_, index) => {
        const indicator = document.createElement('div');
        indicator.className = `indicator ${index === currentIndex ? 'active' : ''}`;
        indicator.addEventListener('click', () => goToSlide(index));// Mouse enter event for preview
        indicator.addEventListener('mouseenter', function(e) {
            showPreview(index, e.target);
        });

        // Mouse leave event
        indicator.addEventListener('mouseleave', function() {
            hidePreview();
        });
        indicatorsContainer.appendChild(indicator);
    });
}

// Go to specific slide
function goToSlide(index) {
    currentIndex = index;
    updateCarousel();
    resetAutoSlide();
}

// Show next slide
function showNextSlide() {
    currentIndex = (currentIndex + 1) % images.length;
    updateCarousel();
    resetAutoSlide();
}

// Show previous slide
function showPrevSlide() {
    currentIndex = (currentIndex - 1 + images.length) % images.length;
    updateCarousel();
    resetAutoSlide();
}

// Start auto-slide
function startAutoSlide() {
    if (intervalId) clearInterval(intervalId);
    intervalId = setInterval(showNextSlide, 4000);
}

// Reset auto-slide timer
function resetAutoSlide() {
    startAutoSlide();
}