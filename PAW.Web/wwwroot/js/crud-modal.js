// Maneja el modal reutilizable de Add / Edit / Details / Delete.
// - Los enlaces con el atributo data-modal cargan su Partial View dentro del modal.
// - Los formularios con data-modal-form se envían por fetch; si todo sale bien se recarga la página actual
//   (conservando el número de página), y si hay errores se vuelve a pintar el formulario con sus mensajes.
(function () {
    const modalEl = document.getElementById('crudModal');
    if (!modalEl || typeof bootstrap === 'undefined') return;

    const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
    const content = modalEl.querySelector('.modal-content');

    const spinner = '<div class="modal-body text-center py-5"><div class="spinner-border" role="status"></div></div>';
    const errorHtml = '<div class="modal-header"><h5 class="modal-title">Something went wrong</h5>' +
        '<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button></div>' +
        '<div class="modal-body"><div class="alert alert-danger mb-0">The request could not be completed.</div></div>';

    async function load(url) {
        content.innerHTML = spinner;
        modal.show();
        try {
            const response = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } });
            content.innerHTML = await response.text();
        } catch (e) {
            content.innerHTML = errorHtml;
        }
    }

    document.addEventListener('click', function (e) {
        const link = e.target.closest('a[data-modal]');
        if (!link) return;
        e.preventDefault();
        load(link.href);
    });

    document.addEventListener('submit', async function (e) {
        const form = e.target.closest('form[data-modal-form]');
        if (!form) return;
        e.preventDefault();

        const submit = form.querySelector('[type="submit"]');
        if (submit) submit.disabled = true;

        try {
            const response = await fetch(form.action, {
                method: 'POST',
                body: new FormData(form),
                headers: { 'X-Requested-With': 'XMLHttpRequest' }
            });

            if ((response.headers.get('content-type') || '').includes('application/json')) {
                const data = await response.json();
                if (data.success) {
                    modal.hide();
                    window.location.reload();
                    return;
                }
                content.innerHTML = errorHtml;
                return;
            }

            content.innerHTML = await response.text();
        } catch (err) {
            content.innerHTML = errorHtml;
        }
    });
})();
