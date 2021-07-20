//Load Data in Table when documents is ready
$(document).ready(function () {
    loadBenefits();
});

//Load Data function
function loadBenefits() {
    abp.services.app.benefits.listAll({ id: webinarId }).done(function (result) {
        console.log(result);
        var html = '';
        $.each(result, function (key, item) {
            html += '<tr>';
            html += '<td>' + item.id + '</td>';
            html += '<td>' + item.name + '</td>';
            html += '<td>' + item.description + '</td>';
            html += '<td><a href="javascript:void(0)" onclick="return getBenefitbyID(' + item.id + ')"><i class="fa fa-pencil"></i></a> | <a href="javascript:void(0)" style="color:red" onclick="DeleleBenefit(' + item.id + ')"><i class="fa fa-trash"></i></a></td>';
            html += '</tr>';
        });
        if (result.length == 0) {
            html += '<tr>';
            html += '<td colspan="4">No recods found</td>';
            html += '</tr>';
        }
        $('.tbody').html(html);
    });
}

//Add Data Function 
function AddBenefit() {
    var res = validateBenefit();
    if (res == false) {
        return false;
    }
    var empObj = {
        id: 0,
        name: $('#BenefitName').val(),
        description: $('#Description').val(),
        webinarId: webinarId
    };
    abp.services.app.benefits.add(empObj).done(function (result) {
        $('#myModal').modal('hide');
        loadBenefits();
        clearBenefitTextBox();
    });
}

//Function for getting the Data Based upon Employee ID
function getBenefitbyID(EmpID) {
    $('#BenefitName').css('border-color', 'lightgrey');
    $('#Description').css('border-color', 'lightgrey');
    abp.services.app.benefits.getbyID({ id: EmpID }).done(function (result) {
        $('#BenefitID').val(result.id);
        $('#BenefitName').val(result.name);
        $('#Description').val(result.description);

        $('#myModal').modal('show');
        $('#btnUpdate').show();
        $('#btnAdd').hide();
    });

    return false;
}

//function for updating employee's record
function UpdateBenefit() {
    var res = validateBenefit();
    if (res == false) {
        return false;
    }
    var empObj = {
        id: $('#BenefitID').val(),
        name: $('#BenefitName').val(),
        description: $('#Description').val()
    };
    abp.services.app.benefits.update(empObj).done(function (result) {
        loadBenefits();
        $('#myModal').modal('hide');
        clearBenefitTextBox();
    });
}

//function for deleting employee's record
function DeleleBenefit(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        abp.services.app.benefits.deleteBenefit({ id: ID }).done(function (result) {
            loadBenefits();
        });
    }
}

//Function for clearing the textboxes
function clearBenefitTextBox() {
    $('#BenefitID').val("");
    $('#BenefitName').val("");
    $('#Description').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#BenefitName').css('border-color', 'lightgrey');
    $('#Description').css('border-color', 'lightgrey');
}
//Valdidation using jquery
function validateBenefit() {
    var isValid = true;
    if ($('#BenefitName').val().trim() == "") {
        $('#BenefitName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#BenefitName').css('border-color', 'lightgrey');
    }
    if ($('#Description').val().trim() == "") {
        $('#Description').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Description').css('border-color', 'lightgrey');
    }

    return isValid;
}