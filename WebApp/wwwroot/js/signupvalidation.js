document.addEventListener('DOMContentLoaded', () => {
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

                    return;
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
})

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}