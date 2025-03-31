// wwwroot/js/appointment-dashboard.js
function initializeAppointmentDashboard(hourlyData, trendData, statusData, counts) {
    // Daily appointments chart
    var hourlyDataArray = Object.keys(hourlyData).map(function (hour) {
        return hourlyData[hour];
    });

    var options1 = {
        chart: { type: 'bar', height: 50, sparkline: { enabled: true } },
        colors: ['#4680FF'],
        plotOptions: { bar: { columnWidth: '80%' } },
        series: [{ data: hourlyDataArray }],
        xaxis: { crosshairs: { width: 1 } },
        tooltip: {
            fixed: { enabled: false },
            x: { show: false },
            y: {
                title: {
                    formatter: function (seriesName) {
                        return '';
                    }
                }
            },
            marker: { show: false }
        }
    };
    var chart1 = new ApexCharts(document.querySelector('#daily-appointment-graph'), options1);
    chart1.render();

    // Weekly appointments chart
    var weeklyDataArray = Object.keys(trendData).slice(-7).map(function (date) {
        return trendData[date] || 0;
    });

    var options2 = {
        chart: { type: 'bar', height: 50, sparkline: { enabled: true } },
        colors: ['#E58A00'],
        plotOptions: { bar: { columnWidth: '80%' } },
        series: [{ data: weeklyDataArray }],
        xaxis: { crosshairs: { width: 1 } },
        tooltip: {
            fixed: { enabled: false },
            x: { show: false },
            y: {
                title: {
                    formatter: function (seriesName) {
                        return '';
                    }
                }
            },
            marker: { show: false }
        }
    };
    var chart2 = new ApexCharts(document.querySelector('#weekly-appointment-graph'), options2);
    chart2.render();

    // Monthly appointments chart
    var monthlyDataArray = Object.keys(trendData).map(function (date) {
        return trendData[date] || 0;
    });

    var options3 = {
        chart: { type: 'bar', height: 50, sparkline: { enabled: true } },
        colors: ['#2CA87F'],
        plotOptions: { bar: { columnWidth: '80%' } },
        series: [{ data: monthlyDataArray }],
        xaxis: { crosshairs: { width: 1 } },
        tooltip: {
            fixed: { enabled: false },
            x: { show: false },
            y: {
                title: {
                    formatter: function (seriesName) {
                        return '';
                    }
                }
            },
            marker: { show: false }
        }
    };
    var chart3 = new ApexCharts(document.querySelector('#monthly-appointment-graph'), options3);
    chart3.render();

    // Status appointments chart
    var statusLabels = Object.keys(statusData);
    var statusValues = statusLabels.map(function (status) {
        return statusData[status];
    });

    var options4 = {
        chart: { type: 'donut', height: 50, sparkline: { enabled: true } },
        colors: ['#DC2626', '#E58A00', '#2CA87F', '#4680FF'],
        series: statusValues,
        labels: statusLabels,
        tooltip: {
            fixed: { enabled: false },
            y: {
                title: {
                    formatter: function (seriesName) {
                        return seriesName + ':';
                    }
                }
            },
            marker: { show: false }
        }
    };
    var chart4 = new ApexCharts(document.querySelector('#status-appointment-graph'), options4);
    chart4.render();

    // Appointment trends graph
    var trendDates = Object.keys(trendData);
    var trendValues = trendDates.map(function (date) {
        return trendData[date] || 0;
    });

    var options5 = {
        chart: {
            type: 'area',
            height: 300,
            toolbar: { show: false }
        },
        colors: ['#0d6efd'],
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                type: 'vertical',
                inverseColors: false,
                opacityFrom: 0.5,
                opacityTo: 0
            }
        },
        dataLabels: { enabled: false },
        stroke: { width: 1 },
        grid: { strokeDashArray: 4 },
        series: [{ name: 'Appointments', data: trendValues }],
        xaxis: {
            categories: trendDates.map(function (date) {
                return new Date(date).toLocaleDateString();
            }),
            axisBorder: { show: false },
            axisTicks: { show: false }
        }
    };
    var chart5 = new ApexCharts(document.querySelector('#appointment-trend-graph'), options5);
    chart5.render();

    // Total appointments graph
    var options6 = {
        chart: {
            type: 'area',
            height: 60,
            stacked: true,
            sparkline: { enabled: true }
        },
        colors: ['#4680FF'],
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                type: 'vertical',
                inverseColors: false,
                opacityFrom: 0.5,
                opacityTo: 0
            }
        },
        stroke: { curve: 'smooth', width: 2 },
        series: [{ data: monthlyDataArray }]
    };
    var chart6 = new ApexCharts(document.querySelector('#total-appointment-graph'), options6);
    chart6.render();

    // Pending appointments graph
    var pendingTrend = [0, counts.pending / 2, counts.pending / 4, counts.pending / 3, counts.pending / 5, counts.pending / 2, counts.pending];
    var options7 = {
        chart: {
            type: 'area',
            height: 60,
            stacked: true,
            sparkline: { enabled: true }
        },
        colors: ['#DC2626'],
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                type: 'vertical',
                inverseColors: false,
                opacityFrom: 0.5,
                opacityTo: 0
            }
        },
        stroke: { curve: 'smooth', width: 2 },
        series: [{ data: pendingTrend }]
    };
    var chart7 = new ApexCharts(document.querySelector('#pending-appointment-graph'), options7);
    chart7.render();
}