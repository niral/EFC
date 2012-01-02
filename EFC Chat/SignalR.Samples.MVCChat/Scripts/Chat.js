﻿/// <reference path="../../Scripts/jquery-1.6.2.js" />
/// <reference path="../../Scripts/jQuery.tmpl.js" />
/// <reference path="../../Scripts/jquery.cookie.js" />
var m = 1;


$(function () {

    var chat = $.connection.chat;


    function clearMessages() {
        $('#messages').html('');
    }
    function clearUsers() {
        $('#users').html('');
    }

    function addMessage(content, type) {
        var e = $('<li/>').html(content).appendTo($('#messages'));
        if (type) {
            e.addClass(type);
        }
        updateUnread();
        e[0].scrollIntoView();
        return e;
    }

    chat.refreshRoom = function (room) {
        clearMessages();
        clearUsers();

        chat.getUsers()
            .done(function (users) {
                $.each(users, function () {
                    chat.addUser(this, true);
                });

                $('#new-message').focus();
            });

        addMessage('Entered ' + room, 'notification');
    };

    chat.showRooms = function (rooms) {
        if (!rooms.length) {
            addMessage('No rooms available', 'notification')
        }
        else {
            $.each(rooms, function () {
                addMessage(this.Name + ' (' + this.Count + ')');
            });
        }
    };

    chat.addMessageContent = function (id, content) {
        var e = $('#m-' + id).append(content);
        updateUnread();
        e[0].scrollIntoView();
    };

    chat.addMessage = function (id, name, message) {
        var data = {
            name: name,
            message: message,
            id: id
        };

        var e = $('#new-message-template').tmpl(data)
                                          .appendTo($('#messages'));
        updateUnread();
        e[0].scrollIntoView();
    };

    chat.addUser = function (user, exists) {
        var id = 'u-' + user.Name;
        if (document.getElementById(id)) {
            return;
        }

        var data = {
            name: user.Name,
            hash: user.Hash
        };

        var e = $('#new-user-template').tmpl(data)
                                       .appendTo($('#users'));

        if (!exists && this.name != user.Name) {
            addMessage(user.Name + ' just entered ' + this.room, 'notification');
            e.hide().fadeIn('slow');
        }

        updateCookie();
    };

    chat.changeUserName = function (oldUser, newUser) {
        $('#u-' + oldUser.Name).replaceWith(
                $('#new-user-template').tmpl({
                    name: newUser.Name,
                    hash: newUser.Hash
                })
        );

        if (oldUser.Name === this.name) {
            addMessage('Your name is now ' + newUser.Name, 'notification');
            updateCookie();
        }
        else {
            addMessage(oldUser.Name + '\'s nick has changed to ' + newUser.Name, 'notification');
        }
    };

    chat.sendMeMessage = function (name, message) {
        addMessage('*' + name + '* ' + message, 'notification');
    };

    chat.sendPrivateMessage = function (from, to, message) {
        addMessage('<emp>*' + from + '*</emp> ' + message, 'pm');
    };

    chat.leave = function (user) {
        if (this.id != user.Id) {
            $('#u-' + user.Name).fadeOut('slow', function () {
                $(this).remove();
            });

            addMessage(user.Name + ' left ' + this.room, 'notification');
        }
    };

    $('#send-message').submit(function () {
        var command = $('#new-message').val();
        //    command = '/nick idan';
        chat.send(command)
            .fail(function (e) {
                addMessage(e, 'error');
            });
        if (m == 1) {

        }
        alert(m);
        m++;

        //   m = m + 1;
        /*
        if ($b == false) {
        chat.send("/join Idan");
        $b = true;
        }*/
        $('#new-message').val('');
        $('#new-message').focus();

        return false;
    });

    $(window).blur(function () {
        chat.focus = false;
    });

    $(window).focus(function () {
        chat.focus = true;
        chat.unread = 0;
        document.title = 'EFC Chat';
    });

    function updateUnread() {
        if (!chat.focus) {
            if (!chat.unread) {
                chat.unread = 0;
            }
            chat.unread++;
        }
        updateTitle();
    }

    function updateTitle() {
        if (chat.unread == 0) {
            document.title = 'EFC Chat';
        }
        else {
            document.title = 'EFC Chat (' + chat.unread + ')';
        }
    }

    function updateCookie() {
        $.cookie('userid', chat.id, { path: '/', expires: 30 });
    }

    addMessage('Welcome to EFC CHAT', 'notification');

    $('#new-message').val('');
    $('#new-message').focus();

    $.connection.hub.start(function () {
        chat.join()
            .done(function (success) {
                if (success === false) {
                    $.cookie('userid', '')
                    addMessage('Choose a name using "/nick nickname".', 'notification');
                }
                addMessage('After that, you can view rooms using "/rooms" and join a room using "/join roomname".', 'notification');
            });
    });
});