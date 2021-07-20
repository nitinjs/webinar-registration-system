//Load Data in Table when documents is ready
$(document).ready(function () {
    loadTestimonials();
});

//Load Data function
function loadTestimonials() {
    abp.services.app.testimonials.listAll({ id: webinarId }).done(function (result) {
        console.log(result);
        var html = '';
        $.each(result, function (key, item) {
            html += '<tr>';
            html += '<td>' + item.id + '</td>';
            html += '<td><img src="' + item.photoPath + '" style="width:50px;"/></td>';
            html += '<td>' + item.name + '</td>';
            html += '<td>' + item.review + '</td>';
            html += '<td><a href="javascript:void(0)" onclick="return getTestimonialbyID(' + item.id + ')"><i class="fa fa-pencil"></i></a> | <a style="color:red" href="javascript:void(0)" onclick="DeleleTestimonial(' + item.id + ')"><i class="fa fa-trash"></i></a></td>';
            html += '</tr>';
        });
        if (result.length == 0) {
            html += '<tr>';
            html += '<td colspan="5">No recods found</td>';
            html += '</tr>';
        }
        $('.tbodyTM').html(html);
    });
}

//Add Data Function 
function AddTestimonial() {
    var res = validateTestimonial();
    if (res == false) {
        return false;
    }

    var formData = new FormData();
    var totalFiles = document.getElementById("TestimonialPhoto").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("TestimonialPhoto").files[i];
        formData.append("SpeakerProfilePhoto", file);
    }
    $.ajax({
        type: "POST",
        url: '/Home/Upload',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false
        //success: function(response) {
        //    alert('succes!!');
        //},
        //error: function(error) {
        //    alert("errror");
        //}
    }).done(function (obj) {
        var empObj = {
            id: 0,
            name: $('#TestimonialName').val(),
            photoPath: obj.result.path,
            review: $('#TestimonialDescription').val(),
            numberOfLikes: $('#NumberOfLikes').val(),
            numberOfReviews: $('#NumberOfReviews').val(),
            numberOfStars: $('#NumberOfStars').val(),
            webinarId: webinarId
        };
        abp.services.app.testimonials.add(empObj).done(function (result) {
            $('#myModalTM').modal('hide');
            loadTestimonials();
            clearTestimonialTextBox();
        });
    }).fail(function (xhr, status, errorThrown) {
        alert('fail');
    });
}

//Function for getting the Data Based upon Employee ID
function getTestimonialbyID(EmpID) {
    $('#TestimonialName').css('border-color', 'lightgrey');
    $('#TestimonialPhoto').css('border-color', 'lightgrey');
    $('#TestimonialDescription').css('border-color', 'lightgrey');
    $('#NumberOfLikes').css('border-color', 'lightgrey');
    $('#NumberOfReviews').css('border-color', 'lightgrey');
    $('#NumberOfStars').css('border-color', 'lightgrey');

    abp.services.app.testimonials.getbyID({ id: EmpID }).done(function (obj) {
        $('#TestimonialID').val(obj.id);
        $('#TestimonialName').val(obj.name);
        $('#imgPhotoTestimonial').attr("src", obj.photoPath);
        $('#TestimonialDescription').val(obj.review);
        $('#NumberOfLikes').val(obj.numberOfLikes);
        $('#NumberOfReviews').val(obj.numberOfReviews);
        $('#NumberOfStars').val(obj.numberOfStars);

        $('#myModalTM').modal('show');
        $('#btnUpdateTestimonial').show();
        $('#btnAddTestimonial').hide();
    });

    return false;
}

//function for updating employee's record
function UpdateTestimonial() {
    var res = validateTestimonial();
    if (res == false) {
        return false;
    }

    if ($('#TestimonialPhoto').val().trim() != "") {
        var formData = new FormData();
        var totalFiles = document.getElementById("TestimonialPhoto").files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("TestimonialPhoto").files[i];
            formData.append("TestimonialPhoto", file);
        }
        $.ajax({
            type: "POST",
            url: '/Home/Upload',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false
            //success: function(response) {
            //    alert('succes!!');
            //},
            //error: function(error) {
            //    alert("errror");
            //}
        }).done(function (obj) {
            var empObj = {
                id: $('#TestimonialID').val(),
                name: $('#TestimonialName').val(),
                photoPath: $('#TestimonialPhoto').val().trim() == "" ? $("#imgPhotoTestimonial").attr("src") : obj.result.path,
                review: $('#TestimonialDescription').val(),
                numberOfLikes: $('#NumberOfLikes').val(),
                numberOfReviews: $('#NumberOfReviews').val(),
                numberOfStars: $('#NumberOfStars').val()
            };
            abp.services.app.testimonials.update(empObj).done(function (result) {
                loadTestimonials();
                $('#myModalTM').modal('hide');
                clearTestimonialTextBox();
            });
        }).fail(function (xhr, status, errorThrown) {
            alert('fail');
        });
    } else {
        var empObj = {
            id: $('#TestimonialID').val(),
            name: $('#TestimonialName').val(),
            photoPath: $('#TestimonialPhoto').val().trim() == "" ? $("#imgPhotoTestimonial").attr("src") : obj.result.path,
            review: $('#TestimonialDescription').val(),
            numberOfLikes: $('#NumberOfLikes').val(),
            numberOfReviews: $('#NumberOfReviews').val(),
            numberOfStars: $('#NumberOfStars').val()
        };
        abp.services.app.testimonials.update(empObj).done(function (result) {
            loadTestimonials();
            $('#myModalTM').modal('hide');
            clearTestimonialTextBox();
        });
    }
}

//function for deleting employee's record
function DeleleTestimonial(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        abp.services.app.testimonials.deleteTestimonial({ id: ID }).done(function (result) {
            loadTestimonials();
        });
    }
}

//Function for clearing the textboxes
function clearTestimonialTextBox() {
    $('#TestimonialID').val("");
    $('#TestimonialName').val("");
    $('#TestimonialPhoto').val("");
    $('#TestimonialDescription').val("");
    $('#NumberOfLikes').val("1");
    $('#NumberOfReviews').val("1");
    $('#NumberOfStars').val("1");
    $('#btnUpdateTestimonial').hide();
    $('#btnAddTestimonial').show();
    $("#imgPhotoTestimonial").attr("src", "/img/man.png");

    $('#TestimonialName').css('border-color', 'lightgrey');
    $('#TestimonialPhoto').css('border-color', 'lightgrey');
    $('#TestimonialDescription').css('border-color', 'lightgrey');
    $('#NumberOfLikes').css('border-color', 'lightgrey');
    $('#NumberOfReviews').css('border-color', 'lightgrey');
    $('#NumberOfStars').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validateTestimonial() {
    var isValid = true;
    if ($('#TestimonialName').val().trim() == "") {
        $('#TestimonialName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TestimonialName').css('border-color', 'lightgrey');
    }

    if ($("#btnAddTestimonial").is(":visible")) {
        if ($('#TestimonialPhoto').val().trim() == "") {
            $('#TestimonialPhoto').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#TestimonialPhoto').css('border-color', 'lightgrey');
        }
    }

    if ($('#TestimonialDescription').val().trim() == "") {
        $('#TestimonialDescription').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#TestimonialDescription').css('border-color', 'lightgrey');
    }

    
    if ($('#NumberOfLikes').val().trim() == "") {
        $('#NumberOfLikes').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NumberOfLikes').css('border-color', 'lightgrey');
    }
    if ($('#NumberOfReviews').val().trim() == "") {
        $('#NumberOfReviews').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#NumberOfReviews').css('border-color', 'lightgrey');
    }

    return isValid;
}