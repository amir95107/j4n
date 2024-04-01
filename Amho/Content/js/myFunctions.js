var loader = '<div class="lds-roller"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>';
var loader2 = '<div class="lds-ring"><div></div><div></div><div></div><div></div><div></div><div></div><div></div><div></div></div>';

const changeImage = (event) => {
    const target = event.currentTarget;
    var src = target.getAttribute('data-src');
    document.getElementById('currentImg').src = 'content/img/' + src + '.png';
    document.querySelectorAll('.intro--options > a').forEach(e => {
        e.classList.remove('is-active');
    })
    target.classList.add('is-active');
    // if($(window).width()<769){
    //     $('#allModal').modal('show');
    // }
}

const handleAboutItem = (event) => {
    const dataItem = event.currentTarget.getAttribute('data-target');
    var element = document.querySelectorAll(".am_about-item");
    const tabs = document.querySelectorAll('.about--options a');
    element.forEach(e => {
        e.classList.remove('am_about-item-is-active');
    });
    document.querySelector('.am_about-item[data-item=' + dataItem + ']').classList.add('am_about-item-is-active');
    tabs.forEach(e => {
        e.classList.remove('am_tab-is-active');
    });
    event.currentTarget.classList.add('am_tab-is-active');
    // element.classList = element.classList.replace(/\am_about-item-is-active\b/g, "");
    // document.querySelectorAll('.am_about-item[data-item='+ dataItem +']')
}

const handleSubmit = event => {
    event.preventDefault();
    if (confirm('لطفا جهت همکاری یکی از راه های بخش تماس را انتخاب کنید')) {

        $('li[data-item="contact"]').click();
    }
};

amirHosseinSays = (txt) => {
    alert(txt);
}

touchFunction = () => {
    $('#amir').attr('onclick', 'touchAgain()')
    amirHosseinSays('دست نزن :))');
}

let count = 1;
touchAgain = () => {

    count++;
    if (count === 3) {
        $('#amir').attr('src', '/Content/img/amir2.png');
        amirHosseinSays('بیا ، انقد دست زدی بهم کج شدم!');
        return false;
    }
    amirHosseinSays('دوباره که دست زدی :)))');
}

$(window).on('resize', () => {
    if ($(this).width() < 950 && $(this).width() > 600 && window.location.hash == '#about') {
        amirHosseinSays('جام خیلی تنگه ! لطفا یکم صفحرو بزرگتر کن!!');
    }
})

function sendRequest(e) {
    showModal('ارسال درخواست', loader, "text-dark");
    setTimeout(() => {
        $.ajax({
            url: "/Home/CreateRequest",
            type: "get",
            data: { name: $('#name').val(), email: $('#email').val(), mobile: $('#mobile').val(), rItems: $('#requestItems').attr('class') }
        }).done(function () {
            try {
                showModal('ارسال درخواست', 'درخواست شما با موفقیت ارسال شد؛منتظر تماس ما باشید.', true)
            } catch {
                showModal('ارسال درخواست', 'خطایی رخ داده!', false)
            }
        });
    }, 0)
    e.preventDefault();
}

var checkbox = $('.options-a input[type="checkbox"]');

checkbox.on('input',
    function (e) {
        if (e.target.checked) {
            $('#requestItems').addClass(e.target.value);
        } else {
            $('#requestItems').removeClass(e.target.value);
        }
    });

showModal = (title, message, isSuccess = true) => {
    $('#modal').modal('show');
    $('#modal-body').html(message);
    $('#modal-title').html(title);
    if (isSuccess) {
        $('.modal-content').addClass('modal-success')
    } else {
        $('.modal-content').addClass('modal-error')
    }
}

getFollowers = (name) => {
    $('#modal').modal('show');
    $('#modal').find('.modal-body').html(loader);
    $.get('/Instagram/GetFollowers', (res) => {
        $('.modal-body').html(res)
    })
}

showFollowUserForm = (isFollow,username) => {
    $('#modal').modal('show');
    $('#modal').find('.modal-body').html(loader);

    $.get('/Instagram/FollowUserFollowerModal?isFollow=' + isFollow, (res) => {
        $('.modal-body').html(res).find('#username').val(username);
    })
}

startTimer = (selector) => {
    var timer = 0;
    setInterval(() => {
        timer++;
        $(selector).html(timer)
    }, 1000)
}

$.ajax({})

followUserFollowers = (e) => {
    e.preventDefault();
    $('#timerWapper').html('')
    var username = $('#username').val();
    var count = $('#count').val();
    $('.modal-body').html(loader);
    var timer = `<span id="timer">0</span>`;
    var timerWrapper = `<div class="text-center text-dark" id="timerWapper">زمان:${timer}</div>`;
    $('.modal-body').append(timerWrapper);
    startTimer('#timer')
    $.ajax({
        url: '/Instagram/FollowUserFollower',
        type: 'Post',
        data: { username: username, count: count },
        success: (res) => {
            $('.modal-body').html(res);
        },
        error: err => console.log(err),
        finally: () => {
            $('#timerWapper').html('')
        }
    })

}

unfollowUserFollowers = (e) => {
    e.preventDefault();
    $('#timerWapper').html('')
    var count = $('#count').val();
    $('.modal-body').html(loader);
    var timer = `<span id="timer">0</span>`;
    var timerWrapper = `<div class="text-center text-dark">زمان:${timer}</div>`;
    $('.modal-body').append(timerWrapper);
    startTimer('#timer')
    $.ajax({
        url: '/Instagram/UnfollowMyFollowings',
        type: 'Post',
        data: { count: count },
        success: (res) => {
            $('.modal-body').html(res);
        },
        error: err => { $('.modal-body').html(err); },
        finally: () => {
            $('#timerWapper').html('')
        }
    })
}


    $('.header--nav-toggle').click(function () {

        $('.perspective').addClass('perspective--modalview');
        setTimeout(function () {
            $('.perspective').addClass('effect-rotate-left--animate');
        }, 25);
        $('.outer-nav, .outer-nav li, .outer-nav--return').addClass('is-vis');

    });

    $('.outer-nav--return, .outer-nav li').click(function () {

        $('.perspective').removeClass('effect-rotate-left--animate');
        setTimeout(function () {
            $('.perspective').removeClass('perspective--modalview');
        }, 400);
        $('.outer-nav, .outer-nav li, .outer-nav--return').removeClass('is-vis');

    });

$('#modal').on('hidden.bs.modal', function () {
    $('#modal-body').html('')
})

showFWNf = () => {
    $('#modal').modal('show');
    $('#modal-body').html(loader);
    $.get('/instagram/ShowFollowingWhichNotFollowers', (res) => {
        $('#modal-body').html(res);

    })
}

showUserPosts = (username) => {
    $('#modal-body').html(loader);

    $.get('/instagram/showUserPosts?username=' + username, (res) => {
        $('#modal-body').html(res);
    })
}

unfollowUser = (e, username) => {
    var a = $(e.currentTarget);
    var count = parseInt($('#countFNG').html());
    a.html(loader2);
    $('.lds-ring').addClass('lds-ring-small')
    $.get('/instagram/UnFollowUser?username=' + username, (res) => {
        if (res) {
            count -= 1;
            $('#countFNG').html(count)
            a.html('Unfollowed').addClass('btn-success').removeClass('btn-danger');
        } else {
            a.html('retry');
        }
    })
}