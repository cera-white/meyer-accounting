$('#business-services').owlCarousel({
    items: 3,
    margin: 10, 
    loop: false, 
    nav: true,
    navText: [
      "<i class='icon-chevron-left icon-white'></i>",
      "<i class='icon-chevron-right icon-white'></i>"
    ],
    dots: false,
    responsive: {
        0: {
            items: 1
        },
        479: {
            items: 1
        },
        768: {
            items: 2
        },
        979: {
            items: 3
        },
        1199: {
            items: 3
        }
    }
}).addClass('owl-theme').addClass('owl-carousel-init');

$('#individual-services').owlCarousel({
    items: 3,
    margin: 10,
    loop: false,
    nav: true,
    navText: [
      "<i class='icon-chevron-left icon-white'></i>",
      "<i class='icon-chevron-right icon-white'></i>"
    ],
    dots: false,
    responsive: {
        0: {
            items: 1
        },
        479: {
            items: 1
        },
        768: {
            items: 2
        },
        979: {
            items: 3
        },
        1199: {
            items: 3
        }
    }
}).addClass('owl-theme').addClass('owl-carousel-init');

function applyCollapse() {
    var width = $(window).outerWidth();

    console.log('Window with is: ', width);

    if (width < 767) {
        $('#home').addClass('collapse');
        console.log('Adding collapse class');
    } else {
        $('#home').removeClass('collapse');
        console.log('Removing collapse class');
    }
}

$(function () {
    //add class active to currently selected nav link
    $('#mainNav li a').each(function () {
        var a = $(this);

        if (a.prop('href') === window.location.href) {
            a.closest('li').addClass('active');
        }
    });

    //apply collapse class when screen is small
    applyCollapse();

    $(window).on('resize', function () {
        applyCollapse();
    });

    $('#commentForm').submit(function (event) {
        var textVal = $('#commentForm input.confirm')[0].val().trim().toLowerCase();

        if (textVal && textVal !== '8' && textVal !== 'eight') {
            event.preventDefault();
        }
        else {
            var target = event.target || event.srcElement;
            $(target.id).submit();
        }
    });

    $('#commentForm').validate({
        rules: {
            'UserName': {
                required: true,
                maxlength: 500
            },
            'Comment1': {
                required: true,
                maxlength: 1000
            }
        },
        messages: {
            'UserName': {
                required: '*Name is required',
                maxlength: 'Name must be less than 500 characters.'
            },
            'Comment1': {
                required: '*Comment is required',
                maxlength: 'Comment must be less than 1000 characters.',
            }
        }
    });

    $('form.newsletterForm').validate({
        rules: {
            'Email': {
                required: true,
                email: true,
                maxlength: 150
            }
        },
        messages: {
            'Email': {
                required: '*Email address is required',
                email: 'Please enter a valid email address',
                maxlength: 'Email must be less than 150 characters'
            }
        }
    });

    $('.subscribeBtn').click(function (event) {
        var form = $(this).closest("form.newsletterForm");
        var textVal = form.find('input.form-control.confirm').val().trim().toLowerCase();

        if (form.valid() && !(textVal && textVal !== '8' && textVal !== 'eight')) {
            var data = form.serialize();

            $.ajax({
                url: '/Newsletters/AddSubscriber',
                type: 'post',
                data: data,
                success: function (data) {
                    if (data.success) {
                        form.find('input.form-control').val('');
                        form.find('.newsletterSuccess').text(data.message).removeClass('hidden');
                        form.find('.newsletterError').addClass('hidden');
                    } else {
                        form.find('.newsletterSuccess').addClass('hidden');
                        form.find('.newsletterError').text(data.message).removeClass('hidden');
                    }
                },
                error: function (request, status, error) {
                    var message = error || 'Oops! Something went wrong. Please look over the form and try again.';
                    form.find('.newsletterSuccess').addClass('hidden');
                    form.find('.newsletterError').text(message).removeClass('hidden');
                }
            });
        }
    });

    $('form.contactForm').validate({
        rules: {
            'Name': {
                required: true,
                maxlength: 150
            },
            'Email': {
                required: true,
                email: true,
                maxlength: 150
            },
            'Subject': {
                required: true,
                maxlength: 200
            },
            'Message': {
                required: true,
                maxlength: 5000
            },
        },
        messages: {
            'Name': {
                required: '*Name is required',
                maxlength: 'Name must be less than 150 characters'
            },
            'Email': {
                required: '*Email address is required',
                email: 'Please enter a valid email address',
                maxlength: 'Email must be less than 150 characters'
            },
            'Subject': {
                required: '*Subject is required',
                maxlength: 'Subject must be less than 200 characters'
            },
            'Message': {
                required: '*Message is required',
                maxlength: 'Message must be less than 5000 characters'
            },
        }
    });

    $('.sendMessageBtn').click(function (event) {
        var form = $(this).closest("form.contactForm");
        var textVal = form.find('input.form-control.confirm').val().trim().toLowerCase();

        if (form.valid() && !(textVal && textVal !== '8' && textVal !== 'eight')) {
            var data = form.serialize();

            $.ajax({
                url: '/Home/SendMessage',
                type: 'post',
                data: data,
                success: function (data) {
                    if (data.success) {
                        form.find('input.form-control').val('');
                        form.find('textarea.form-control').val('');
                        form.find('.contactSuccess').text(data.message).removeClass('hidden');
                        form.find('.contactError').addClass('hidden');
                    } else {
                        form.find('.contactSuccess').addClass('hidden');
                        form.find('.contactError').text(data.message).removeClass('hidden');
                    }
                },
                error: function (request, status, error) {
                    form.find('.contactSuccess').addClass('hidden');
                    form.find('.contactError').text(error).removeClass('hidden');
                }
            });
        }
    });

    $('.collapse-control > button').click(function (event) {
        var button = $(this);
        var collapsedLabel = 'View More <i class="fa fa-angle-down"></i>';
        var expandedLabel = 'View Less <i class="fa fa-angle-up"></i>';

        if (button.hasClass('collapsed')) {
            button.html(expandedLabel);
            button.removeClass('collapsed');
        } else {
            button.html(collapsedLabel);
            button.addClass('collapsed');
        }
    });
});