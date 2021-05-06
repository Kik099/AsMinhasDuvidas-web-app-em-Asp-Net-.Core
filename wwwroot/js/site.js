// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

jQueryAjaxDelete = form => {
    if (confirm('Tem a certeza que pretende eliminar ?')) {


        try {
            $.ajax({

                type: 'POST',
                url: form.action,
                success: function (res) {
                    $("#").html(res.html);
                },
                error: function (err) {
                    console.log(err);
                }

            })

        }
        catch (e) {
            console.log(e); 
        }





    }
    return false;

}