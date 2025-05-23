﻿document.addEventListener('DOMContentLoaded', () => {

    const previewSize = 150

    const editButtons = document.querySelectorAll('.option.edit')
    editButtons.forEach(button => {
        button.addEventListener('click', () => {
            document.querySelector('#editProjectForm_Id').value = button.dataset.id
        })
    })

    //open modals
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {

            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';
        })
    })

    //close modals
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')
            if (modal) {
                modal.style.display = 'none';

                //clear formdata
                modal.querySelectorAll('form').forEach(form => {
                    form.reset()

                    const imagePreview = form.querySelector('.image-preview')
                    if (imagePreview)
                        imagePreview.src = ''

                    const imagePreviewer = form.querySelector('.image-previewer')
                    if (imagePreviewer)
                        imagePreviewer.classList.remove('selected')
                })
            }

        })
    })

    //open/close options
    const optionButtons = document.querySelectorAll('[data-option="true"]')
    optionButtons.forEach(button => {
        button.addEventListener('click', () => {
            const optionTarget = button.getAttribute('data-target')
            const option = document.querySelector(optionTarget)

            if (option) {
                if (getComputedStyle(option).display === 'grid')
                    option.style.display = 'none';
                else
                    option.style.display = 'grid';
            }

        })
    })


     //handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]')
        const imagePreview = previewer.querySelector('.image-preview')

        if (!fileInput || !imagePreview)
            return

        previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })

    // handle submit forms
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    
                    const modal = form.closest('.modal')
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload()
                }
                else if (res.status === 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            let errorSpan = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (errorSpan) {
                                errorSpan.innerText = data.errors[key].join('\n')
                                errorSpan.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('Failed to submit form')
            }
        })
    })

    //handle delete-project buttons

    document.querySelectorAll('.delete-button').forEach(button => {
        button.addEventListener('click', async function () {
            const projectId = this.getAttribute('id').replace('deleteProject-', '');

            const confirmDelete = confirm('Are you sure you want to delete this project?');
            if (!confirmDelete)
                return

            try {

                const res = await fetch(`/Project/DeleteProject/${projectId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },

                })

                const data = await res.json()
               

                if (data.success) {
                    window.location.reload()
                    return;
                } else {
                    alert('Failed to delete project: ' + data.message)
                    return;
                }


            } catch (error) {
                console.error('Failed to delete project:', error)
            }

        })
    })


    //initDropdowns();

})

// Haven't added any dropdowns yet so I chose to comment this out for now

//function initDropdowns() {

//    document.addEventListener('click', (event) => {

//        let clickedInsideDropdown = false;

//        document.querySelectorAll('[data-type="dropdown"]').forEach(dropdownTrigger => {
//            const targetId = dropdownTrigger.getAttribute('data-target')
//            const dropdown = document.querySelector(targetId)

//            if (dropdownTrigger.contains(event.target)) {
//                clickedInsideDropdown = true;

//                document.querySelectorAll('.dropdown.show').forEach(openDropdown => {
//                    if (openDropdown !== dropdown) {
//                        openDropdown.classList.remove('show');
//                    }
//                })

//                dropdown?.classList.toggle('show')

//            }
           
//        })

//        if (!clickedInsideDropdown && !event.target.closest('.dropdown')) {
//            document.querySelectorAll('.dropdown.show').forEach(openDropdown => {
//                openDropdown.classList.remove('show')
//            })
//        }
//    })
//}


function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file"))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load image"))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }

        reader.readAsDataURL(file)
    })
}

async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error('Failed on image-processing:', error)
    }
}