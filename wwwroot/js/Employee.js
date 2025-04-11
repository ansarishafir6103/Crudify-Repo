function viewHobbies(id) {
    $.get('/Home/LoadHobbies?id=' + id, function (data) {
        if (data && data.length > 0) {
            var listHTML = "<ul>";
            data.forEach((item) => {
                listHTML += `<li>${item}</li>`;
            });
            listHTML += "</ul>";
            $('.modal-body').html(listHTML);
        } else {
            $('.modal-body').html("<p>No hobbies found.</p>");
        }

        var modal = new bootstrap.Modal(document.getElementById('exampleModal'));
        modal.show();
    });
}
