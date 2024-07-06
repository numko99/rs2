import 'package:flutter/material.dart';
import 'package:iter_mobile/models/filters.dart';

class ChipFilter extends StatefulWidget {
  final Function() onFiltersChanged;
  final Filter filter;

  const ChipFilter(
      {Key? key, required this.onFiltersChanged, required this.filter})
      : super(key: key);

  @override
  _ChipFilterState createState() => _ChipFilterState();
}

class _ChipFilterState extends State<ChipFilter> {
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Wrap(
        alignment: WrapAlignment.start,
        spacing: 8.0,
        runSpacing: 4.0,
        children: widget.filter.activeFilters
            .map((filterTemp) => Chip(
                  backgroundColor: Colors.amber,
                  label: Text(filterTemp.values.first,
                      style: const TextStyle(color: Colors.white, fontSize: 10)),
                  onDeleted: () {
                    setState(() {
                      if (filterTemp.keys.first == 'date') {
                        widget.filter.startDate = null;
                        widget.filter.endDate = null;
                      } else if (filterTemp.keys.first == 'rating') {
                        widget.filter.rating = null;
                      }
                    });
                    widget.onFiltersChanged();
                  },
                  deleteIcon: const Icon(Icons.cancel, color: Colors.white, size: 15),
                ))
            .toList(),
      ),
    );
  }
}
