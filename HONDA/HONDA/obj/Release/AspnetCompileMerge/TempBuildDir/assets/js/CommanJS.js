 
function mypathurl() {
    mypath = "";
    return mypath
}


function myDateFormatter(dateObject) {
    var d = new Date(moment(dateObject, '"DD-MMM-YYYY').format());
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var date = year + "/" + month + "/" + day;

    return date;
};




//Calendar date Picker
$(function () { 
    $('.mycal').prop('readonly', true);
    $(".mycal").css("background-color", "white");
    $(".mycal").datepicker({ dateFormat: "dd-M-yy", changeMonth: true, changeYear: true});
});
//Calendar time Picker
$(function () {
    $('.timepicker').datetimepicker({
        format: 'HH:mm'
    });
});

//decimal validation number 5  & 3 decimal
$(function () {
    $('.mynumvalthreedigit').on('input', function () {
        match = (/(\d{0,3})[^.]*((?:\d{0,0})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
        this.value = match[1] + match[2];
    });
});
//decimal validation number 5   only
 $(function () {
            $('.mynumval').on('input', function () {
                match = (/(\d{0,5})[^.]*((?:\.\d{0,2})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
                this.value = match[1] + match[2];
     });
});
////Decimal validation number 10 & 2 decimal
// $(function () { 
//     $('.advanceloanmynumval').on('input', function () {
//         if (this.value.length > 10) {
//             this.value = this.value ;
//         } else {
//             match = (/(\d{0,10})[^.]*((?:\.\d{0,2})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
//             this.value = match[1] + match[2];
//         }
//     });
// });
 //Decimal only
 function check(e, value) {
     //Check Charater
     var unicode = e.charCode ? e.charCode : e.keyCode;
     if (value.indexOf(".") != -1)
         if (unicode == 46) return false;
     if (unicode != 8)
         if ((unicode < 48 || unicode > 57) && unicode != 46) return false;
 }
//decimal validation
 function NumberValdation(ctl) {
     var keycode = window.event.keyCode;
     if ((keycode >= 48 && keycode <= 57) && (keycode != 45) || (keycode == 46 && nval.indexOf('.') == -1)) {
         return true;
     }
     else {
         return false;
     }
 }


// Number Only
 $(function () {
     $('.numberonlyjs').on('input', function () {
         if (this.value.length > 10) {
             this.value = this.value;
         } else {
             match = (/(\d{0,10})[^.]*((?:\d{0,0})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
             this.value = match[1] + match[2];
         }
     });
 });

// Number Only
 $(function () {
     $('.numberonlyjs').on('input', function () {
         if (this.value.length > 10) {
             this.value = this.value;
         } else {
             match = (/(\d{0,10})[^.]*((?:\d{0,10})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
             this.value = match[1] + match[2];
         }
     });
 });


 //Decimal validation number 10 & 2 decimal
 $(function () {
     $('.myDecimal').on('input', function () {
         if (this.value.length != 0) {
             if (this.value.length > 10) {
                 this.value = this.value;
             } else {
                 match = (/(\d{0,10})[^.]*((?:\.\d{0,2})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
                 this.value = match[1] + match[2];
             }
         }
         else {
            
             setTimeout(function () { this.focus(); });
         }
     });
 });

 //$(function () {
 //    $('.mynumvaltandigit').on('input', function () {
 //        alert(this.value);
 //        match = (/(\d{0,5})[^.]*((?:\.\d{0,3})?)/g).exec(this.value.replace(/[^\d.]/g, ''));
 //        this.value = match[1] + match[2];
 //    });
 //});


//$(function () {
//    swal({
//        title: "Ajax request example",
//        text: "Submit to run ajax request",
//        type: "info",
//        showCancelButton: true,
//        closeOnConfirm: false,
//        showLoaderOnConfirm: true
//    }, function () {
//        //setTimeout(function () {
//        //    swal("Ajax request finished!");
//        //}, 2000);
//    });
//});
 

//function NumberValdation(ctl) {
//   // alert(ctl);
//   // var keycode = window.event.keyCode;
//   //var nval = $('#'+ctl).val();   
//   // if ((keycode >= 48 && keycode <= 57) && (keycode != 45) || (keycode == 46 && nval.indexOf('.') == -1 && nval.substring(nval.indexOf('.')).length > 2)) {
//   //     return true;
//   // }
//   // else {
//   //     return false;
//   // }

//    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
//        //alert('hello');
//        if ((event.which != 46 || $(this).val().indexOf('.') != -1)) {
//            alert('Multiple Decimals are not allowed');
//        }
//        event.preventDefault();
//    }
//    if (document.getElementById(ctl).value.indexOf(".") > -1 && (document.getElementById(ctl).value.split('.')[1].length > 1)) {
//        alert('Two numbers only allowed after decimal point');
//        event.preventDefault();
//    }
//}


//Alert eg onclick="showSwal('submit')"
(function ($) {
    showSwal = function (type) {
        'use strict';

        if (type === 'import') {
            swal({
                title: 'Are you sure?',
                text: "You want to Import the File?",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3f51b5',
                cancelButtonColor: '#ff4081',
                confirmButtonText: 'Great ',
                buttons: {
                    cancel: {
                        text: "Cancel",
                        value: null,
                        visible: true,
                        className: "btn btn-danger",
                        closeModal: true,
                    },
                    confirm: {
                        text: "OK",
                        value: true,
                        visible: true,
                        className: "btn btn-primary",
                        closeModal: true
                    }
                }
            })


        }

        else if (type === 'Reject') {          
            swal({
            
                 
                title: "<p  style='font-size:15px !important; float:left !important'>Remarks</p>",
                text: "<textarea class='form-control' style='border-radius: 3px;' id='text'></textarea>",
                // --------------^-- define html element with id
                html: true,
                showCancelButton: true,                
                cancelButtonText: 'Cancel ',
                confirmButtonText: 'Ok ',
                cancelButtonClass: 'btn btn-sm btn-danger',
                confirmButtonClass: 'btn btn-sm btn-primary',
            }, function (inputValue) {
                //if (inputValue === false) return false;
                //if (inputValue === "") {
                //    swal.showInputError("You need to write something!");
                //    return false
                //}
                //// get value using textarea id
               // var val = document.getElementById('text').value;
                //swal("Nice!", "You wrote: " + val, "success");
                 
                return true
            });

               
        }

             else if (type === 'submit') {
                swal({
                    title: 'Are you sure?',
                    text: "You want to submit the Request?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3f51b5',
                    cancelButtonColor: '#ff4081',
                    confirmButtonText: 'Ok ',
                    buttons: {
                        cancel: {
                            text: "Cancel",
                            value: null,
                            visible: true,
                            className: "btn btn-danger",
                            closeModal: true,
                        },
                        confirm: {
                            text: "OK",
                            value: true,
                            visible: true,
                            className: "btn btn-primary",
                            closeModal: true
                        }
                    }
                })


            }

        //else if (type === 'yesno') {
        //    swal({
        //        title: 'Are you sure',
        //        text: "You want to withdraw the applied leave ?",
        //        icon: 'warning',
        //        showCancelButton: true,
        //        confirmButtonColor: '#3f51b5',
        //        cancelButtonColor: '#ff4081',
        //        cancelButtonText: 'No ',
        //        confirmButtonText: 'Yes ', 
        //        cancelButtonClass: 'btn btn-sm btn-danger',
        //        confirmButtonClass: 'btn btn-sm btn-primary',  
        //        //buttons: {
        //        //    cancel: {
        //        //        text: "No",
        //        //        value: null,
        //        //        visible: true,
        //        //        className: "btn btn-danger",
        //        //        closeModal: true,
        //        //    },
        //        //    confirm: {
        //        //        text: "Yes",
        //        //        value: true,
        //        //        visible: true,
        //        //        className: "btn btn-primary",
        //        //        closeModal: true
        //        //    }
        //        //}
        //    })


        //}
            else if (type === 'update') {
                //swal({
                //    title: 'Are you sure?',
                //    text: "You want to Update the Request?",
                //    icon: 'warning',
                //    showCancelButton: true,
                //    confirmButtonColor: '#3f51b5',
                //    cancelButtonColor: '#ff4081',
                //    confirmButtonText: 'Great ',
                //    buttons: {
                //        cancel: {
                //            text: "Cancel",
                //            value: null,
                //            visible: true,
                //            className: "btn btn-danger",
                //            closeModal: true,
                //        },
                //        confirm: {
                //            text: "OK",
                //            value: true,
                //            visible: true,
                //            className: "btn btn-primary",
                //            closeModal: true
                //        }
                //    }
                //})
                swal({
                    title: "Ajax request example",
                    text: "Submit to run ajax request",
                    type: "info",
                    showCancelButton: true,
                    closeOnConfirm: false,
                    showLoaderOnConfirm: true
                  }, function () {
                    //setTimeout(function () {
                    //    swal("Ajax request finished!");
                    //}, 2000);
                });
            
            }
            else if (type === 'Edit') {
                swal({
                    title: 'Are you sure?',
                    text: "You want to Edit the item?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3f51b5',
                    cancelButtonColor: '#ff4081',
                    confirmButtonText: 'Great ',
                    buttons: {
                        cancel: {
                            text: "Cancel",
                            value: null,
                            visible: true,
                            className: "btn btn-danger",
                            closeModal: true,
                        },
                        confirm: {
                            text: "OK",
                            value: true,
                            visible: true,
                            className: "btn btn-primary",
                            closeModal: true

                        }
                    }
                })


            }

            else if (type === 'delete') {
                swal({
                    title: 'Are you sure?',
                    text: "You want to delete the item?",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3f51b5',
                    cancelButtonColor: '#ff4081',
                    confirmButtonText: 'Great ',
                    buttons: {
                        cancel: {
                            text: "Cancel",
                            value: null,
                            visible: true,
                            className: "btn btn-danger",
                            closeModal: true,
                        },
                        confirm: {
                            text: "OK",
                            value: true,
                            visible: true,
                            className: "btn btn-primary",
                            closeModal: true
                        }
                    }
                })

            }
            else if (type === 'basic') {
                swal({
                    text: 'Any fool can use a computer',
                    button: {
                        text: "OK",
                        value: true,
                        visible: true,
                        className: "btn btn-primary"
                    }
                })

            }
            else if (type === 'title-and-text') {
                swal({
                    title: 'Read the alert!',
                    text: 'Click OK to close this alert',
                    button: {
                        text: "OK",
                        value: true,
                        visible: true,
                        className: "btn btn-primary"
                    }
                })

        }
            else if (type === 'success-message') {
                swal({
                    title: 'Congratulations!',
                    text: 'You entered the correct answer',
                    icon: 'success',
                    button: {
                        text: "Continue",
                        value: true,
                        visible: true,
                        className: "btn btn-primary"
                    }
                })

        }
            else if (type === 'auto-close') {
                swal({
                    title: 'Auto close alert!',
                    text: 'I will close in 2 seconds.',
                    timer: 2000,
                    button: false
                }).then(
                    function () { },
                    // handling the promise rejection
                    function (dismiss) {
                        if (dismiss === 'timer') {
                            console.log('I was closed by the timer')
                        }
                    }
                    )
        }
            else if (type === 'warning-message-and-cancel') {
                swal({
                    title: 'Are you sure?',
                    text: "You won't be able to revert this!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3f51b5',
                    cancelButtonColor: '#ff4081',
                    confirmButtonText: 'Great ',
                    buttons: {
                        cancel: {
                            text: "Cancel",
                            value: null,
                            visible: true,
                            className: "btn btn-danger",
                            closeModal: true,
                        },
                        confirm: {
                            text: "OK",
                            value: true,
                            visible: true,
                            className: "btn btn-primary",
                            closeModal: true
                        }
                    }
                })

        }
            else if (type === 'custom-html') {
                swal({
                    content: {
                        element: "input",
                        attributes: {
                            placeholder: "Type your password",
                            type: "password",
                            class: 'form-control'
                        },
                    },
                    button: {
                        text: "OK",
                        value: true,
                        visible: true,
                        className: "btn btn-primary"
                    }
                })
            }
    }

})(jQuery);