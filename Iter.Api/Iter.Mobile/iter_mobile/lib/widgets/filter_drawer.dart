import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:intl/intl.dart';
import 'package:iter_mobile/models/filters.dart';

class FilterDrawer extends StatefulWidget {
  final Function(Filter) onFiltersChanged;
  final Filter filter;

  const FilterDrawer(
      {Key? key, required this.onFiltersChanged, required this.filter})
      : super(key: key);

  @override
  _FilterDrawerState createState() => _FilterDrawerState();
}

class _FilterDrawerState extends State<FilterDrawer> {
  DateTime? startDate;
  DateTime? endDate;
  double? rating;

void initState() {
    super.initState();
    if (widget.filter.startDate != null) {
      startDate = widget.filter.startDate;
    }
    if (widget.filter.endDate != null) {
      endDate = widget.filter.endDate;
    }
    if (widget.filter.rating != null) {
      rating = widget.filter.rating;
    }
  }

  Future<void> _selectDateRange(BuildContext context) async {
    final DateTimeRange? picked = await showDateRangePicker(
      context: context,
      firstDate: DateTime.now().subtract(const Duration(days: 365)),
      lastDate: DateTime.now().add(const Duration(days: 365)),
      initialDateRange: startDate != null && endDate != null
          ? DateTimeRange(start: startDate!, end: endDate!)
          : null,
    );
    if (picked != null && picked.start != picked.end) {
      setState(() {
        startDate = picked.start;
        endDate = picked.end;
      });
    }
  }

void updateFilters() {
    Filter updatedFilter = Filter(startDate: startDate, endDate: endDate, rating: rating);

    widget.onFiltersChanged(updatedFilter);
  }

  void removeFilters() {
    widget.filter.clear();
    widget.onFiltersChanged(widget.filter);
  }


  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: Column(
        children: [
          Expanded(
            child: ListView(
              children: <Widget>[
                const SizedBox(height: 30),
                Padding(
                  padding: const EdgeInsets.all(16.0),
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text("Filteri",
                          style: TextStyle(fontWeight: FontWeight.bold)),
                      IconButton(
                        icon: const Icon(Icons.close),
                        onPressed: () {
                          Navigator.pop(context);
                        },
                      )
                    ],
                  ),
                ),
                const Divider(),
                ExpansionTile(
                  title: const Text('Odabir datuma'),
                  children: <Widget>[
                    ListTile(
                      title: const Text('Odaberite raspon datuma'),
                      onTap: () => _selectDateRange(context),
                      subtitle: startDate != null && endDate != null
                          ? Text(
                              "${DateFormat('dd.MM.yyyy').format(startDate!.toLocal())} - ${DateFormat('dd.MM.yyyy').format(endDate!.toLocal())}")
                          : null,
                    ),
                  ],
                ),
                ExpansionTile(
                  title: const Text('Ocjena agencije'),
                  children: <Widget>[
                    const SizedBox(height: 5),
                    RatingBar.builder(
                      initialRating: rating ?? 0,
                      itemSize: 30,
                      minRating: 1,
                      allowHalfRating: true,
                      direction: Axis.horizontal,
                      itemCount: 5,
                      itemPadding: const EdgeInsets.symmetric(horizontal: 4.0),
                      itemBuilder: (context, _) => const Icon(
                        Icons.star,
                        color: Colors.amber,
                      ),
                      onRatingUpdate: (ratingTemp) {
                        rating = ratingTemp;
                      },
                    ),
                    const SizedBox(height: 5),
                  ],
                )
              ],
            ),
          ),
          ListTile(
            title: const Center(child: Text('Osvježi')),
            onTap: () {
              removeFilters();
              Navigator.pop(context);
            },
          ),
          Container(
            color: Colors.amber,
            child: ListTile(
              title: const Center(
                  child:
                      Text('Primijeni', style: TextStyle(color: Colors.white))),
              onTap: () async {
                updateFilters();
                Navigator.pop(context);
              },
            ),
          ),
        ],
      ),
    );
  }
}
