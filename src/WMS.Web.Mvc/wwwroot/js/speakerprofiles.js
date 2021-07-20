//Load Data in Table when documents is ready
$(document).ready(function () {
    loadSpeakerProfiles();
});

//Load Data function
function loadSpeakerProfiles() {
    abp.services.app.speakerProfiles.listAll({ id: webinarId }).done(function (result) {
        console.log(result);
        var html = '';
        $.each(result, function (key, item) {
            html += '<tr>';
            html += '<td>' + item.id + '</td>';
            html += '<td><img src="' + item.photoPath + '" style="width:50px;"/></td>';
            html += '<td>' + item.name + '</td>';
            html += '<td>' + item.website + '</td>';
            html += '<td>' + item.description + '</td>';
            html += '<td><a href="javascript:void(0)" onclick="return getSpeakerProfilebyID(' + item.id + ')"><i class="fas fa-pencil-alt"></i></a> | <a href="javascript:void(0)" style="color:red" onclick="DeleleSpeakerProfile(' + item.id + ')"><i class="fas fa-trash"></i></a></td>';
            html += '</tr>';
        });
        if (result.length == 0) {
            html += '<tr>';
            html += '<td colspan="6">No recods found</td>';
            html += '</tr>';
        }
        $('.tbodySP').html(html);
    });
}

//Add Data Function 
function AddSpeakerProfile() {
    var res = validateSpeakerProfile();
    if (res == false) {
        return false;
    }

    var formData = new FormData();
    var totalFiles = document.getElementById("SpeakerProfilePhoto").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("SpeakerProfilePhoto").files[i];
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
            name: $('#SpeakerProfileName').val(),
            description: $('#SpeakerProfileDescription').val(),
            photoPath: obj.result.path,
            website: $('#SpeakerProfileWebsite').val(),
            webinarId: webinarId
        };
        abp.services.app.speakerProfiles.add(empObj).done(function (result) {
            $('#myModalSP').modal('hide');
            loadSpeakerProfiles();
            clearSpeakerProfileTextBox();
        });
    }).fail(function (xhr, status, errorThrown) {
        alert('fail');
    });
}

//Function for getting the Data Based upon Employee ID
function getSpeakerProfilebyID(EmpID) {
    $('#SpeakerProfileName').css('border-color', 'lightgrey');
    $('#SpeakerProfileDescription').css('border-color', 'lightgrey');
    $('#SpeakerProfilePhoto').css('border-color', 'lightgrey');
    $('#SpeakerProfileWebsite').css('border-color', 'lightgrey');

    abp.services.app.speakerProfiles.getbyID({ id: EmpID }).done(function (obj) {
        $('#SpeakerProfileID').val(obj.id);
        $('#SpeakerProfileName').val(obj.name);
        $('#SpeakerProfileDescription').val(obj.description);
        $('#imgPhoto').attr("src", obj.photoPath);        
        $('#SpeakerProfileWebsite').val(obj.website);

        $('#myModalSP').modal('show');
        $('#btnUpdateSpeakerProfile').show();
        $('#btnAddSpeakerProfile').hide();
    });

    return false;
}

//function for updating employee's record
function UpdateSpeakerProfile() {
    var res = validateSpeakerProfile();
    if (res == false) {
        return false;
    }

    if ($('#SpeakerProfilePhoto').val().trim() != "") {
        var formData = new FormData();
        var totalFiles = document.getElementById("SpeakerProfilePhoto").files.length;
        for (var i = 0; i < totalFiles; i++) {
            var file = document.getElementById("SpeakerProfilePhoto").files[i];
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
                id: $('#SpeakerProfileID').val(),
                name: $('#SpeakerProfileName').val(),
                description: $('#SpeakerProfileDescription').val(),
                photoPath: $('#SpeakerProfilePhoto').val().trim() == "" ? $("#imgPhoto").attr("src") : obj.result.path,
                website: $('#SpeakerProfileWebsite').val()
            };
            abp.services.app.speakerProfiles.update(empObj).done(function (result) {
                loadSpeakerProfiles();
                $('#myModalSP').modal('hide');
                clearSpeakerProfileTextBox();
            });
        }).fail(function (xhr, status, errorThrown) {
            alert('fail');
        });
    } else {
        var empObj = {
            id: $('#SpeakerProfileID').val(),
            name: $('#SpeakerProfileName').val(),
            description: $('#SpeakerProfileDescription').val(),
            photoPath: $('#SpeakerProfilePhoto').val().trim() == "" ? $("#imgPhoto").attr("src") : obj.result.path,
            website: $('#SpeakerProfileWebsite').val()
        };
        abp.services.app.speakerProfiles.update(empObj).done(function (result) {
            loadSpeakerProfiles();
            $('#myModalSP').modal('hide');
            clearSpeakerProfileTextBox();
        });
    }
}

//function for deleting employee's record
function DeleleSpeakerProfile(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        abp.services.app.speakerProfiles.deleteSpeakerProfile({ id: ID }).done(function (result) {
            loadSpeakerProfiles();
        });
    }
}

//Function for clearing the textboxes
function clearSpeakerProfileTextBox() {
    $('#SpeakerProfileID').val("");
    $('#SpeakerProfileName').val("");
    $('#SpeakerProfileDescription').val("");
    $('#SpeakerProfilePhoto').val("");
    $('#SpeakerProfileWebsite').val("");
    $('#btnUpdateSpeakerProfile').hide();
    $('#btnAddSpeakerProfile').show();
    $("#imgPhoto").attr("src", "/img/man.png");

    $('#SpeakerProfileName').css('border-color', 'lightgrey');
    $('#SpeakerProfileDescription').css('border-color', 'lightgrey');
    $('#SpeakerProfilePhoto').css('border-color', 'lightgrey');
    $('#SpeakerProfileWebsite').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validateSpeakerProfile() {
    var isValid = true;
    if ($('#SpeakerProfileName').val().trim() == "") {
        $('#SpeakerProfileName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SpeakerProfileName').css('border-color', 'lightgrey');
    }
    if ($('#SpeakerProfileDescription').val().trim() == "") {
        $('#SpeakerProfileDescription').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SpeakerProfileDescription').css('border-color', 'lightgrey');
    }

    if ($("#btnAddSpeakerProfile").is(":visible")) {
        if ($('#SpeakerProfilePhoto').val().trim() == "") {
            $('#SpeakerProfilePhoto').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#SpeakerProfilePhoto').css('border-color', 'lightgrey');
        }
    }

    /*
    if ($('#SpeakerProfileWebsite').val().trim() == "") {
        $('#SpeakerProfileWebsite').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SpeakerProfileWebsite').css('border-color', 'lightgrey');
    }
    */

    return isValid;
}