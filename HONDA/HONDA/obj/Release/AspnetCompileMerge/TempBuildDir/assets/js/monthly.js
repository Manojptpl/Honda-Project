/*
Monthly 2.2.2 by Kevin Thornbloom is licensed under a Creative Commons Attribution-ShareAlike 4.0 International License.
*/

(function ($) {
	"use strict";
	$.fn.extend({
		monthly: function(customOptions) {

			// These are overridden by options declared in footer
			var defaults = {
				dataType: "xml",
				disablePast: false,
				eventList: true,
				events: "",
				jsonUrl: "",
				linkCalendarToEventUrl: false,
				maxWidth: false,
				mode: "event",
				setWidth: false,
				showTrigger: "",
				startHidden: false,
				stylePast: false,
				target: "",
				useIsoDateFormat: false,
				weekStart: 0,	// Sunday
				xmlUrl: ""
			};

			var	options = $.extend(defaults, customOptions),
				uniqueId = $(this).attr("id"),
				parent = "#" + uniqueId,
				currentDate = new Date(),
				currentMonth = currentDate.getMonth() + 1,
				currentYear = currentDate.getFullYear(),
				currentDay = currentDate.getDate(),
				locale = (options.locale || defaultLocale()).toLowerCase(),
				monthNameFormat = options.monthNameFormat || "short",
				weekdayNameFormat = options.weekdayNameFormat || "short",
				monthNames = options.monthNames || defaultMonthNames(),
				dayNames = options.dayNames || defaultDayNames(),
                markupBlankDay = '<div class="m-d monthly-day-blank"><div class="monthly-day-number"></div></div>',
               
				weekStartsOnMonday = options.weekStart === "Mon" || options.weekStart === 1 || options.weekStart === "1",
                primaryLanguageCode = locale.substring(0, 2).toLowerCase();
                

		if (options.maxWidth !== false) {
			$(parent).css("maxWidth", options.maxWidth);
		}
		if (options.setWidth !== false) {
			$(parent).css("width", options.setWidth);
		}

		if (options.startHidden) {
			$(parent).addClass("monthly-pop").css({
				display: "none",
				position: "absolute"
			});
			$(document).on("focus", String(options.showTrigger), function (event) {
				$(parent).show();
				event.preventDefault();
			});
			$(document).on("click", String(options.showTrigger) + ", .monthly-pop", function (event) {
				event.stopPropagation();
				event.preventDefault();
			});
			$(document).on("click", function () {
				$(parent).hide();
			});
		}

		// Add Day Of Week Titles
		_appendDayNames(weekStartsOnMonday);

		// Add CSS classes for the primary language and the locale. This allows for CSS-driven
		// overrides of the language-specific header buttons. Lowercased because locale codes
		// are case-insensitive but CSS is not.
		$(parent).addClass("monthly-locale-" + primaryLanguageCode + " monthly-locale-" + locale);

		// Add Header & event list markup
		$(parent).prepend('<div class="monthly-header"><div class="monthly-header-title"><a href="#" class="monthly-header-title-date" onclick="return false"></a></div><a href="#" class="monthly-prev"></a><a href="#" class="monthly-next"></a></div>').append('<div class="monthly-event-list"></div>');

		// Set the calendar the first time
		setMonthly(currentMonth, currentYear);

		// How many days are in this month?
		function daysInMonth(month, year) {
			return month === 2 ? (year & 3) || (!(year % 25) && year & 15) ? 28 : 29 : 30 + (month + (month >> 3) & 1);
		}

		// Build the month
		function setMonthly(month, year) {
			$(parent).data("setMonth", month).data("setYear", year);

			// Get number of days
			var index = 0,
				dayQty = daysInMonth(month, year),
				// Get day of the week the first day is
				mZeroed = month - 1,
				firstDay = new Date(year, mZeroed, 1, 0, 0, 0, 0).getDay(),
				settingCurrentMonth = month === currentMonth && year === currentYear;

			// Remove old days
			$(parent + " .monthly-day, " + parent + " .monthly-day-blank").remove();
			$(parent + " .monthly-event-list, " + parent + " .monthly-day-wrap").empty();
			// Print out the days
			for(var dayNumber = 1; dayNumber <= dayQty; dayNumber++) {
				// Check if it's a day in the past
				var isInPast = options.stylePast && (
					year < currentYear
					|| (year === currentYear && (
						month < currentMonth
						|| (month === currentMonth && dayNumber < currentDay)
					))),
					innerMarkup = '<div class="monthly-day-number">' + dayNumber + '</div><div class="monthly-indicator-wrap"></div>';
                if (options.mode === "event") {
                    
					var thisDate = new Date(year, mZeroed, dayNumber, 0, 0, 0, 0);
					$(parent + " .monthly-day-wrap").append("<div"
						+ attr("class", "m-d monthly-day monthly-day-event _weekoff"
							+ (isInPast ? " monthly-past-day" : "")
							+ " dt" + thisDate.toISOString().slice(0, 10)
							)
						+ attr("data-number", dayNumber)
						+ ">" + innerMarkup + "</div>");
					$(parent + " .monthly-event-list").append("<div"
						+ attr("class", "monthly-list-item")
						+ attr("id", uniqueId + "day" + dayNumber)
						+ attr("data-number", dayNumber)
						+ '><div class="monthly-event-list-date">' + dayNames[thisDate.getDay()] + "<br>" + dayNumber + "</div></div>");
                } else {
                    
					$(parent + " .monthly-day-wrap").append("<a"
						+ attr("href", "#")
						+ attr("class", "m-d monthly-day monthly-day-pick" + (isInPast ? " monthly-past-day" : ""))
						+ attr("data-number", dayNumber)
						+ ">" + innerMarkup + "</a>");
				}
			}

			if (settingCurrentMonth) {
				$(parent + ' *[data-number="' + currentDay + '"]').addClass("monthly-today");
			}

			// Reset button
			$(parent + " .monthly-header-title").html('<a href="#" class="monthly-header-title-date" onclick="return false">' + monthNames[month - 1] + " " + year + "</a>" + (settingCurrentMonth && $(parent + " .monthly-event-list").hide() ? "" : '<a href="#" class="monthly-reset"></a>'));
            //$(parent + " .monthly-header-title").html('<a href="#" class="monthly-header-title-date" onclick="return false">' + monthNames[month - 1] + " " + year + "</a>" + (settingCurrentMonth && $(parent + " .monthly-event-list").hide() ? "" : ''));

			// Account for empty days at start
			if(weekStartsOnMonday) {
				if (firstDay === 0) {
                    _prependBlankDays(6);
                   
				} else if (firstDay !== 1) {
					_prependBlankDays(firstDay - 1);
				}
			} else if(firstDay !== 7) {
				_prependBlankDays(firstDay);
			}

			// Account for empty days at end
			var numdays = $(parent + " .monthly-day").length,
				numempty = $(parent + " .monthly-day-blank").length,
				totaldays = numdays + numempty,
				roundup = Math.ceil(totaldays / 7) * 7,
                daysdiff = roundup - totaldays;
            
			if(totaldays % 7 !== 0) {
                for (index = 0; index < daysdiff; index++) {
                    
					$(parent + " .monthly-day-wrap").append(markupBlankDay);
                }
             
			}

			// Events
            if (options.mode === "event") {
				addEvents(month, year);
			}
			var divs = $(parent + " .m-d");
            for (index = 0; index < divs.length; index += 7) {
               
				divs.slice(index, index + 7).wrapAll('<div class="monthly-week"></div>');
            }

            setWeekOffDayText(month, year);
          
		}

		function addEvent(event, setMonth, setYear) {
			// Year [0]   Month [1]   Day [2]
			var fullStartDate = _getEventDetail(event, "startdate"),
				fullEndDate = _getEventDetail(event, "enddate"),
				startArr = fullStartDate.split("-"),
				startYear = parseInt(startArr[0], 10),
				startMonth = parseInt(startArr[1], 10),
				startDay = parseInt(startArr[2], 10),
				startDayNumber = startDay,
				endDayNumber = startDay,
				showEventTitleOnDay = startDay,
				startsThisMonth = startMonth === setMonth && startYear === setYear,
				happensThisMonth = startsThisMonth;

			if(fullEndDate) {
				// If event has an end date, determine if the range overlaps this month
				var	endArr = fullEndDate.split("-"),
					endYear = parseInt(endArr[0], 10),
					endMonth = parseInt(endArr[1], 10),
					endDay = parseInt(endArr[2], 10),
					startsInPastMonth = startYear < setYear || (startMonth < setMonth && startYear === setYear),
					endsThisMonth = endMonth === setMonth && endYear === setYear,
					endsInFutureMonth = endYear > setYear || (endMonth > setMonth && endYear === setYear);
				if(startsThisMonth || endsThisMonth || (startsInPastMonth && endsInFutureMonth)) {
					happensThisMonth = true;
					startDayNumber = startsThisMonth ? startDay : 1;
					endDayNumber = endsThisMonth ? endDay : daysInMonth(setMonth, setYear);
					showEventTitleOnDay = startsThisMonth ? startDayNumber : 1;
				}
			}
			if(!happensThisMonth) {
				return;
			}

			var startTime = _getEventDetail(event, "starttime"),
				timeHtml = "",
				eventURL = _getEventDetail(event, "url"),
				eventTitle = _getEventDetail(event, "name"),
				eventClass = _getEventDetail(event, "class"),
				eventColor = _getEventDetail(event, "color"),
				eventId = _getEventDetail(event, "id"),
				customClass = eventClass ? " " + eventClass : "",
				dayStartTag = "<div",
				dayEndTags = "</span></div>";

			if(startTime) {
				var endTime = _getEventDetail(event, "endtime");
				timeHtml = '<div><div class="monthly-list-time-start">' + formatTime(startTime) + "</div>"
					+ (endTime ? '<div class="monthly-list-time-end">' + formatTime(endTime) + "</div>" : "")
					+ "</div>";
			}

			if(options.linkCalendarToEventUrl && eventURL) {
				dayStartTag = "<a" + attr("href", eventURL);
				dayEndTags = "</span></a>";
			}

			var	markupDayStart = dayStartTag
					+ attr("data-eventid", eventId)
					+ attr("title", eventTitle)
					// BG and FG colors must match for left box shadow to create seamless link between dates
					+ (eventColor ? attr("style", "background:" + eventColor + ";color:" + eventColor) : ""),
				markupListEvent = "<a"
					+ attr("href", eventURL)
					+ attr("class", "listed-event" + customClass)
					+ attr("data-eventid", eventId)
					+ (eventColor ? attr("style", "background:" + eventColor) : "")
					+ attr("title", eventTitle)
					+ ">" + eventTitle + " " + timeHtml + "</a>";
			for(var index = startDayNumber; index <= endDayNumber; index++) {
				var doShowTitle = index === showEventTitleOnDay;
				// Add to calendar view
				$(parent + ' *[data-number="' + index + '"] .monthly-indicator-wrap').append(
						markupDayStart
						+ attr("class", "monthly-event-indicator" + customClass
							// Include a class marking if this event continues from the previous day
							+ (doShowTitle ? "" : " monthly-event-continued")
							)
						+ "><span>" + (doShowTitle ? eventTitle : "") + dayEndTags);
				// Add to event list
				$(parent + ' .monthly-list-item[data-number="' + index + '"]')
					.addClass("item-has-event")
					.append(markupListEvent);

                var textval = $(parent + ' .monthly-list-item[data-number="' + index + '"]').find('a').text()

                if (textval.trim() == 'Sick Leave') {
                    // $(parent + ' .monthly-list-item[data-number="' + index + '"]').find('div').remove();
                    $(parent + ' *[data-number="' + index + '"]').addClass("sickleave");
                    $(parent + ' .monthly-day-event[data-number="' + index + '"]').find('div').removeClass("monthly-event-indicator")

                }

            }
        }

		function addEvents(month, year) {
			if(options.events) {
				// Prefer local events if provided
				addEventsFromString(options.events, month, year);
			} else {
				var remoteUrl = options.dataType === "xml" ? options.xmlUrl : options.jsonUrl;
				if(remoteUrl) {
					// Replace variables for month and year to load from dynamic sources
					var url = String(remoteUrl).replace("{month}", month).replace("{year}", year);
					$.get(url, {now: $.now()}, function(data) {
						addEventsFromString(data, month, year);
					}, options.dataType).fail(function() {
						console.error("Monthly.js failed to import " + remoteUrl + ". Please check for the correct path and " + options.dataType + " syntax.");
					});
				}
			}
		}

		function addEventsFromString(events, setMonth, setYear) {
			if (options.dataType === "xml") {
				$(events).find("event").each(function(index, event) {
					addEvent(event, setMonth, setYear);
				});
			} else if (options.dataType === "json") {
				$.each(events.monthly, function(index, event) {
					addEvent(event, setMonth, setYear);
				});
			}
		}

		function attr(name, value) {
			var parseValue = String(value);
			var newValue = "";
			for(var index = 0; index < parseValue.length; index++) {
				switch(parseValue[index]) {
					case "'": newValue += "&#39;"; break;
					case "\"": newValue += "&quot;"; break;
					case "<": newValue += "&lt;"; break;
					case ">": newValue += "&gt;"; break;
					default: newValue += parseValue[index];
				}
			}
			return " " + name + "=\"" + newValue + "\"";
		}

		function _appendDayNames(startOnMonday) {
			var offset = startOnMonday ? 1 : 0,
				dayName = "",
				dayIndex = 0;
			for(dayIndex = 0; dayIndex < 6; dayIndex++) {
				dayName += "<div>" + dayNames[dayIndex + offset] + "</div>";
			}
			dayName += "<div>" + dayNames[startOnMonday ? 0 : 6] + "</div>";
			$(parent).append('<div class="monthly-day-title-wrap">' + dayName + '</div><div class="monthly-day-wrap"></div>');
		}

		// Detect the user's preferred language
		function defaultLocale() {
			if(navigator.languages && navigator.languages.length) {
				return navigator.languages[0];
			}
			return navigator.language || navigator.browserLanguage;
		}

		// Use the user's locale if possible to obtain a list of short month names, falling back on English
		function defaultMonthNames() {
			if(typeof Intl === "undefined") {
				//return ["Jan", "Feb", "Mar", "Apr", "May", "June", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                return ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
			}
			var formatter = new Intl.DateTimeFormat(locale, {month: monthNameFormat});
			var names = [];
			for(var monthIndex = 0; monthIndex < 12; monthIndex++) {
				var sampleDate = new Date(2017, monthIndex, 1, 0, 0, 0);
				names[monthIndex] = formatter.format(sampleDate);
			}
           // return names;
            return ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
		}

		function formatDate(year, month, day) {
			if(options.useIsoDateFormat) {
				return new Date(year, month - 1, day, 0, 0, 0).toISOString().substring(0, 10);
			}
			if(typeof Intl === "undefined") {
				return month + "/" + day + "/" + year;
			}
			return new Intl.DateTimeFormat(locale).format(new Date(year, month - 1, day, 0, 0, 0));
		}

		// Use the user's locale if possible to obtain a list of short weekday names, falling back on English
		function defaultDayNames() {
			if(typeof Intl === "undefined") {
				return ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
			}
			var formatter = new Intl.DateTimeFormat(locale, {weekday: weekdayNameFormat}),
				names = [],
				dayIndex = 0,
				sampleDate = null;
			for(dayIndex = 0; dayIndex < 7; dayIndex++) {
				// 2017 starts on a Sunday, so use it to capture the locale's weekday names
				sampleDate = new Date(2017, 0, dayIndex + 1, 0, 0, 0);
				names[dayIndex] = formatter.format(sampleDate);
            }
            
			return names;
		}

		function _prependBlankDays(count) {
			var wrapperEl = $(parent + " .monthly-day-wrap"),
				index = 0;
			for(index = 0; index < count; index++) {
				wrapperEl.prepend(markupBlankDay);
			}
		}

		function _getEventDetail(event, nodeName) {
			return options.dataType === "xml" ? $(event).find(nodeName).text() : event[nodeName];
		}

		// Returns a 12-hour format hour/minute with period. Opportunity for future localization.
		function formatTime(value) {
			var timeSplit = value.split(":");
			var hour = parseInt(timeSplit[0], 10);
			var period = "AM";
			if(hour > 12) {
				hour -= 12;
				period = "PM";
			} else if (hour == 12) {
				period = "PM";
			} else if(hour === 0) {
				hour = 12;
			}
			return hour + ":" + String(timeSplit[1]) + " " + period;
		}

		function setNextMonth() {
			var	setMonth = $(parent).data("setMonth"),
				setYear = $(parent).data("setYear"),
				newMonth = setMonth === 12 ? 1 : setMonth + 1,
				newYear = setMonth === 12 ? setYear + 1 : setYear;
			setMonthly(newMonth, newYear);
            viewToggleButton();
            
		}

		function setPreviousMonth() {
			var setMonth = $(parent).data("setMonth"),
				setYear = $(parent).data("setYear"),
				newMonth = setMonth === 1 ? 12 : setMonth - 1,
				newYear = setMonth === 1 ? setYear - 1 : setYear;
			setMonthly(newMonth, newYear);
			viewToggleButton();
		}

		// Function to go back to the month view
        function viewToggleButton() {
           
            if ($(parent + " .monthly-event-list").is(":visible")) {
                
                 //change by krishna
                $(parent + " .monthly-event-list").css("display", "none");
                $("#event-modal").modal("show");
                $(parent + " .monthly-cal").remove();
               //  $(parent + " .monthly-header-title").prepend('<a href="#" class="monthly-cal"></a>');
            //end
			
				
			}
		}

		// Advance months
		$(document.body).on("click", parent + " .monthly-next", function (event) {
			setNextMonth();
			event.preventDefault();
		});

		// Go back in months
		$(document.body).on("click", parent + " .monthly-prev", function (event) {
			setPreviousMonth();
			event.preventDefault();
		});

		// Reset Month
		$(document.body).on("click", parent + " .monthly-reset", function (event) {
			$(this).remove();
            setMonthly(currentMonth, currentYear);
            
			viewToggleButton();
			event.preventDefault();
			event.stopPropagation();
		});

		// Back to month view
		$(document.body).on("click", parent + " .monthly-cal", function (event) {
			$(this).remove();
			$(parent + " .monthly-event-list").css("transform", "scale(0)");
			setTimeout(function() {
				$(parent + " .monthly-event-list").hide();
			}, 250);
			event.preventDefault();
		});

		// Click A Day
		$(document.body).on("dblclick touchstart", parent + " .monthly-day", function (event) {
			// If events, show events list
			var whichDay = $(this).data("number");
			if(options.mode === "event" && options.eventList) {
				var	theList = $(parent + " .monthly-event-list"),
					myElement = document.getElementById(uniqueId + "day" + whichDay),
					topPos = myElement.offsetTop;
				theList.show();
				theList.css("transform");
				theList.css("transform", "scale(1)");
				$(parent + ' .monthly-list-item[data-number="' + whichDay + '"]').show();
				theList.scrollTop(topPos);
                viewToggleButton();
                
				if(!options.linkCalendarToEventUrl) {
					event.preventDefault();
				}
			// If picker, pick date
			} else if (options.mode === "picker") {
				var	setMonth = $(parent).data("setMonth"),
					setYear = $(parent).data("setYear");
				// Should days in the past be disabled?
				if($(this).hasClass("monthly-past-day") && options.disablePast) {
					// If so, don't do anything.
					event.preventDefault();
				} else {
					// Otherwise, select the date ...
					$(String(options.target)).val(formatDate(setYear, setMonth, whichDay));
					// ... and then hide the calendar if it started that way
					if(options.startHidden) {
						$(parent).hide();
					}
				}
				event.preventDefault();
			}
		});

		// Clicking an event within the list
		$(document.body).on("click", parent + " .listed-event", function (event) {
			var href = $(this).attr("href");
			// If there isn't a link, don't go anywhere
			if(!href) {
				event.preventDefault();
			}
		});



            //Custom Function Added
        function setWeekOffDayText(month,year) {

           
            var is_weekend = function (date1) {
                var dt = new Date(date1);

                if (dt.getDay() == 6 || dt.getDay() == 0) {
                    return "weekend";
                }
            }

           // var wk = is_weekend('2019-12-01');
           // var wk = is_weekend('2019-12-01');
            var getTot = daysInMonth(month, year); //Get total days in a month
            var sat = new Array();   //Declaring array for inserting Saturdays
            var sun = new Array();   //Declaring array for inserting Sundays

            for (var i = 1; i <= getTot; i++) {    //looping through days in month
                //var newDate = new Date(year, month, i)
                var newDate = new Date(year+"/"+ month+"/"+ i)
                if (newDate.getDay() == 0) {   //if Sunday
                    sun.push(i);
                }
                if (newDate.getDay() == 6) {   //if Saturday
                    sat.push(i);
                }

            }
            

            $('.monthly-day-event .monthly-day-number').append('<span>W. Hrs - 8:00 </span>');
       //    $('#calendar1 .monthly-day-wrap .monthly-week .monthly-day-event').each(function () {

           $('#calendar1 ._weekoff').each(function () {
                var wk = $(this).attr('data-number');
                //var cdiv = $(this).closest('div.monthly-day-number').html();

                for (var i = 0; i <= sun.length; i++) {
                    if (wk == sun[i]) {
                        //$('.monthly-day-event .monthly-day-number').html('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Input Hrs:8:00');
                        var str = ($(this).closest('div').find('div.monthly-day-number').html()).replace('W. Hrs - 8:00', '');
                        $(this).closest('div').find('div.monthly-day-number').html(str);
                        $(this).closest('div').find('div.monthly-indicator-wrap').html("<span class='spanWeekoff'>Weekly Off</span>");
                        //$(this).closest('div').find('div.monthly-indicator-wrap').css("padding-top", "10%");
                        $(this).closest('div').find('div.monthly-indicator-wrap').css("margin", "20px auto");
                        $(this).css('background-color', '#d9d9d9');
                    }
                }
                for (var i = 0; i <= sat.length; i++) {
                    if (wk == sat[i]) {
                        //$('.monthly-day-event .monthly-day-number').html('&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Input Hrs:8:00');
                        var str = ($(this).closest('div').find('div.monthly-day-number').html()).replace('W. Hrs - 8:00', '');
                        $(this).closest('div').find('div.monthly-day-number').html(str);
                        $(this).closest('div').find('div.monthly-indicator-wrap').html("<span class='spanWeekoff'>Weekly Off</span>");

                        // $(this).closest('div').find('div.monthly-indicator-wrap').css("padding-top", "10%");
                        $(this).closest('div').find('div.monthly-indicator-wrap').css("margin", "20px auto");
                      
                        $(this).css('background-color', '#d9d9d9')
                    }
                }
            });



           $('#calendar1 .sickleave').each(function (i)  {

               debugger;

                   //var cdiv = $(this).closest('div.monthly-day-number').html();
                  // var str = ($(this).closest('div').find('div.monthly-day-number').html()).replace('W.Hrs:8:00', '');
               var st = $(this).closest('div').find('div.monthly-day-number').html();
               if (st != undefined) {
                   $(this).closest('div').find('div.monthly-day-number').html(st.replace('W. Hrs - 8:00', ''));
               }
               
            
              
           });
        }
        function daysInMonth(month, year) {
            return new Date(year, month, 0).getDate();
        }
	}
	});
}(jQuery));
