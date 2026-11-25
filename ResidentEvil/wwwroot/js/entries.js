// @ts-nocheck
function filterEntries(category, btnElement) {
    document.querySelectorAll('.entry-row').forEach(row => {
        const rowCat = row.getAttribute('data-category');
        row.style.display = (category === 'all' || rowCat === category) ? '' : 'none';
    });

    document.querySelectorAll('.filter-btn').forEach(btn => btn.classList.remove('active'));
    if (btnElement) {
        btnElement.classList.add('active');
    }
}

const deleteModal = document.getElementById('deleteModal');
if (deleteModal) {
    deleteModal.addEventListener('show.bs.modal', function (event) {
        const btn = event.relatedTarget;
        document.getElementById('deleteName').textContent = btn.getAttribute('data-name');
        document.getElementById('deleteForm').action =
            deleteModal.dataset.deleteUrl + '/' + btn.getAttribute('data-id');
    });
}