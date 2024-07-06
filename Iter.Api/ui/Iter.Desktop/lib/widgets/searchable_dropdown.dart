import 'package:flutter/material.dart';

class SearchableDropdown extends StatefulWidget {
  final Future<List<String>> Function(String) searchFunction;

  const SearchableDropdown({Key? key, required this.searchFunction})
      : super(key: key);

  @override
  _SearchableDropdownState createState() => _SearchableDropdownState();
}

class _SearchableDropdownState extends State<SearchableDropdown> {
  TextEditingController _controller = TextEditingController();
  List<String> _dropdownItems = [];
  String? _selectedItem;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        TextFormField(
          controller: _controller,
          decoration: InputDecoration(
            labelText: 'Search',
            suffixIcon: _controller.text.length > 0
                ? IconButton(
                    icon: Icon(Icons.clear),
                    onPressed: () {
                      _controller.clear();
                      setState(() {
                        _dropdownItems = [];
                      });
                    },
                  )
                : null,
          ),
          onChanged: (value) async {
            if (value.length >= 3) {
              var results = await widget.searchFunction(value);
              setState(() {
                _dropdownItems = results;
              });
            } else {
              setState(() {
                _dropdownItems = [];
              });
            }
          },
        ),
        SizedBox(height: 20),
        if (_dropdownItems.isNotEmpty)
          DropdownButton<String>(
            isExpanded: true,
            value: _selectedItem,
            hint: Text("Select an item"),
            items: _dropdownItems.map<DropdownMenuItem<String>>((String value) {
              return DropdownMenuItem<String>(
                value: value,
                child: Text(value),
              );
            }).toList(),
            onChanged: (String? newValue) {
              setState(() {
                _selectedItem = newValue;
              });
            },
          ),
      ],
    );
  }
}
