google.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);






function drawChart() {
    var data = google.visualization.arrayToDataTable([
        ['Contry', 'Mhl'],
        ['Italy', 54.8],
        ['France', 48.6],
        ['Spain', 44.4],
        ['USA', 23.9],
        ['Argentina', 14.5]
    ]);

    var options = {
        title: 'World Wide Wine Production',
        is3D: true
    };

    var chart = new google.visualization.PieChart(document.getElementById('myChart'));
    chart.draw(data, options);

    var chart = new google.visualization.BarChart(document.getElementById('myChart2'));
    chart.draw(data, options);
}